const eventUri = 'api/Events';
const participantUri = 'api/Participants';
const accountUri = 'api/Account';
const speakerUri = 'api/Speakers';
const accessToken = 'accessToken';
const userId = 'userId';
let eventsList = [];
function getEvents() {
    fetch(eventUri)
        .then(response => response.json())
        .then(data => displayEvents(data)).catch(error => console.error('Unable to get events.', error));
}

async function showSpeakerInfo(event) {
    const speakerModal = document.getElementById('speakerModal');
    const button = event.relatedTarget;
    const speakerid = Number(button.getAttribute('speakerid'));

    let speaker = await fetch(`${speakerUri}/${speakerid}`)
        .then(response => response.json())
        .then(data => {
            return data;
        })
        .catch(error => console.error('Unable to get speaker.', error));

    const firstName = speakerModal.querySelector('#speaker-first-name');
    const lastName = speakerModal.querySelector('#speaker-last-name');
    const email = speakerModal.querySelector('#speaker-email');

    let speakerFirstName = document.createTextNode("Ім'я': " + speaker.firstName);
    let speakerLastName = document.createTextNode("Прізвище: " + speaker.lastName);
    let speakerEmail = document.createTextNode("Пошта: " + speaker.email);
    firstName.innerHTML = '';
    lastName.innerHTML = '';
    email.innerHTML = '';
    firstName.appendChild(speakerFirstName);
    lastName.appendChild(speakerLastName);
    email.appendChild(speakerEmail);

}

function displayEvents(data) {
    const eventContainer = document.getElementById('eventContainer');
    const button = document.createElement('button');
    data.forEach(currentEvent => {

        let headerTextNode = document.createTextNode(currentEvent.title);
        let descriptionTextNode = document.createTextNode(currentEvent.description);
        let addressTextNode = document.createTextNode("Адреса: " + currentEvent.address);
        let startDateTimeTextNode = document.createTextNode("Початок: " + currentEvent.startDateTime);
        let endDateTimeTextNode = document.createTextNode("Кінець: " + currentEvent.endDateTime);

        let headerInfo = document.createElement('h4');
        headerInfo.appendChild(headerTextNode);
        let descriptionInfo = document.createElement('div');
        descriptionInfo.appendChild(descriptionTextNode);
        let addressInfo = document.createElement('div');
        addressInfo.appendChild(addressTextNode);
        let startDateTimeInfo = document.createElement('div');
        startDateTimeInfo.appendChild(startDateTimeTextNode);
        let endDateTimeInfo = document.createElement('div');
        endDateTimeInfo.appendChild(endDateTimeTextNode);

        let row = document.createElement('div');
        row.className = 'row';
        let div = document.createElement('div');
        div.className = 'col-12 card p-3 m-3';
        div.appendChild(headerInfo);
        div.appendChild(descriptionInfo);
        div.appendChild(addressInfo);
        div.appendChild(startDateTimeInfo);
        div.appendChild(endDateTimeInfo);

        let speakerButton = button.cloneNode(false);
        speakerButton.innerText = 'Додаткова інформація';
        speakerButton.setAttribute('data-bs-target', '#speakerModal');
        speakerButton.setAttribute('speakerid', currentEvent.speakerId);
        speakerButton.setAttribute('data-bs-toggle', 'modal');
        speakerButton.className = 'btn btn-primary m-1';

        let signButton = button.cloneNode(false);
        signButton.innerText = 'Записатися';
        signButton.setAttribute('eventid', currentEvent.id);
        signButton.setAttribute('onclick',
            `signEvent(${currentEvent.id})`);
        signButton.className = 'btn btn-primary signEventButton m-1';
        if (sessionStorage.getItem(accessToken) == null){
            signButton.style.display = 'none';
        } else if (sessionStorage.getItem(userId) !== null && currentEvent.participantsIds.find(id => id === Number(sessionStorage.getItem(userId))) !== undefined) {
            signButton.innerText = 'Ви успішно записалися';
            signButton.setAttribute('disabled', '');
        }

        div.appendChild(speakerButton);
        div.appendChild(signButton);

        row.appendChild(div);
        eventContainer.appendChild(row);


    });
    eventsList = data;
}

function displayHeaderButtons() {
    if (sessionStorage.getItem(accessToken) !== null) {
        document.getElementById('loginModalButton').style.display = 'none';
        document.getElementById('registerModalButton').style.display = 'none';
        document.getElementById('logoutButton').style.display = 'block';
    }

}

function signEvent(id) {
    const token = sessionStorage.getItem(accessToken);
    fetch(`${participantUri}/Event?id=${id}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Authorization': 'Bearer ' + token
        }
    })
        .then(async (response) => {
            if (response.ok === true) {
                let buttons = document.getElementsByClassName('signEventButton');
                let currentButton = document.querySelector(`[eventid="` + id + `"]`);
                currentButton.innerText = 'Ви успішно записалися';
                currentButton.setAttribute('disabled', '');
            }
        })
        .catch(error => console.error('Unable to sign.', error));
}

function loginSubmit(event) {
    event.preventDefault();
    fetch(`${accountUri}/Login`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            email: document.getElementById('inputEmail').value,
            password: document.getElementById('inputPassword').value
        })

    })
        .then(async (response) => {
            if (response.ok === true) {
                const data = await response.json();
                sessionStorage.setItem(accessToken, data.token);
                sessionStorage.setItem(userId, data.id);
                document.getElementById('loginModalButton').style.display = 'none';
                document.getElementById('registerModalButton').style.display = 'none';
                document.getElementById('logoutButton').style.display = 'block';
                let buttons = document.getElementsByClassName('signEventButton');
                for (var i = 0; i < buttons.length; i++) {
                    let eventid = Number(buttons[i].getAttribute('eventid'));
                    const currentEvent = eventsList.find(_event => _event.id === eventid);
                    if (currentEvent.participantsIds.find(id => id === data.id) === undefined) {
                        buttons[i].style.display = 'block';
                        buttons[i].innerText = 'Записатися';
                        buttons[i].removeAttribute('disabled');
                    } else {
                        buttons[i].style.display = 'block';
                        buttons[i].innerText = 'Ви успішно записалися';
                        buttons[i].setAttribute('disabled', '');
                    }
                }
                document.getElementById('loginModalContent').style.display = 'none';
                document.getElementById('loginModalSuccess').style.display = 'block';
            } else {
                const errorDiv = document.getElementById('loginModalError');
                errorDiv.innerHTML = "Спробуйте ще раз";
                errorDiv.className = "alert alert-danger";
            }
        })
        .catch(error => console.error('Unable to sign.', error));
}

function logoutSubmit(event) {
    event.preventDefault();
    sessionStorage.removeItem(accessToken);
    sessionStorage.removeItem(userId);
    document.getElementById('loginModalButton').style.display = 'block';
    document.getElementById('registerModalButton').style.display = 'block';
    document.getElementById('logoutButton').style.display = 'none';
    document.getElementById('loginModalContent').style.display = 'block';
    document.getElementById('loginModalSuccess').style.display = 'none';
    let buttons = document.getElementsByClassName('signEventButton');
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].style.display = 'none';
    }
}

function registerSubmit(event) {
    event.preventDefault();
    fetch(`${accountUri}/Register`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            email: document.getElementById('inputRegisterEmail').value,
            password: document.getElementById('inputRegisterPassword').value,
            name: document.getElementById('inputRegisterName').value,
            roles: ['user']
        })

    })
        .then(async (response) => {
            if (response.ok === true) {
                document.getElementById('registerModalContent').style.display = 'none';
                document.getElementById('registerModalSuccess').style.display = 'block';
            } else {
                const errorDiv = document.getElementById('registerModalError');
                errorDiv.innerHTML = "Спробуйте ще раз";
                errorDiv.className = "alert alert-danger";
            }
        })
        .catch(error => console.error('Unable to register.', error));
}


