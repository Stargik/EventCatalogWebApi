using System;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantService participantService;
        private readonly IEventService eventService;

        public ParticipantsController(IParticipantService participantService, IEventService eventService)
        {
            this.participantService = participantService;
            this.eventService = eventService;
        }

        // GET: api/<ParticipantsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipantModel>>> Get()
        {

            var participants = await participantService.GetAllAsync();
            return Ok(participants);
        }

        // GET api/<ParticipantsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantModel>> Get(int id)
        {
            var participant = await participantService.GetByIdAsync(id);
            if (participant is not null)
            {
                return Ok(participant);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/<ParticipantsController>/Event
        [HttpGet("Event")]
        public async Task<ActionResult<IEnumerable<ParticipantModel>>> GetByEvent(int id)
        {
            try
            {
                var participants = await participantService.GetParticipantsByEventIdAsync(id);
                if (participants is not null)
                {
                    return Ok(participants);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        // GET api/<ParticipantsController>/Event
        [HttpPost("Event")]
        [Authorize]
        public async Task<ActionResult> AddEvent(int id)
        {
            try
            {
                var participant = await participantService.GetByEmailAsync(User.Identity.Name);
                if (participant is not null && !participant.EventsIds.Contains(id))
                {
                    participant.EventsIds.Clear();
                    participant.EventsIds.Add(id);
                    await participantService.UpdateAsync(participant);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        // POST api/<ParticipantsController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Add([FromBody] ParticipantModel participant)
        {
            try
            {
                await participantService.AddAsync(participant);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/<ParticipantsController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(int id, [FromBody] ParticipantModel participant)
        {
            try
            {
                participant.Id = id;
                await participantService.UpdateAsync(participant);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/<ParticipantsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await participantService.DeleteAsync(id);
            return Ok();
        }
    }
}
