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

        public ParticipantsController(IParticipantService participantService)
        {
            this.participantService = participantService;
        }

        // GET: api/<ParticipantsController>
        [HttpGet]
        [Authorize(Roles = "admin")]
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

        // POST api/<ParticipantsController>
        [HttpPost("Add")]
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
        [HttpPut("Update/{id}")]
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
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await participantService.DeleteAsync(id);
            return Ok();
        }
    }
}
