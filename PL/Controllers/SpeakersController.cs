using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly ISpeakerService speakerService;

        public SpeakersController(ISpeakerService speakerService)
        {
            this.speakerService = speakerService;
        }

        // GET: api/<SpeakersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpeakerModel>>> Get()
        {
            var speakers = await speakerService.GetAllAsync();
            return Ok(speakers);
        }

        // GET api/<SpeakersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SpeakerModel>> Get(int id)
        {
            var speaker = await speakerService.GetByIdAsync(id);
            if (speaker is not null)
            {
                return Ok(speaker);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/<SpeakersController>/Event
        [HttpGet("Event")]
        public async Task<ActionResult<SpeakerModel>> GetByEvent(int id)
        {
            try
            {
                var speaker = await speakerService.GetSpeakerByEventIdAsync(id);
                if (speaker is not null)
                {
                    return Ok(speaker);
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

        // POST api/<SpeakersController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Add([FromBody] SpeakerModel speaker)
        {
            try
            {
                await speakerService.AddAsync(speaker);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/<SpeakersController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update(int id, [FromBody] SpeakerModel speaker)
        {
            try
            {
                speaker.Id = id;
                await speakerService.UpdateAsync(speaker);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/<SpeakersController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await speakerService.DeleteAsync(id);
            return Ok();
        }
    }
}
