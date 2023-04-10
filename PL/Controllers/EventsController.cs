using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService eventService;

        public EventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        // GET: api/<EventsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> Get()
        {
            var events = await eventService.GetAllAsync();
            return Ok(events);
        }

        // GET api/<EventsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventModel>> Get(int id)
        {
            var _event = await eventService.GetByIdAsync(id);
            if (_event is not null)
            {
                return Ok(_event);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/<EventsController>/Category
        [HttpGet("Category")]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetByCategory(int id)
        {
            var events = await eventService.GetEventsByCategoryIdAsync(id);
            if (events is not null)
            {
                return Ok(events);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/<EventsController>/Speaker
        [HttpGet("Speaker")]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetBySpeaker(int id)
        {
            var events = await eventService.GetEventsBySpeakerIdAsync(id);
            if (events is not null)
            {
                return Ok(events);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<EventsController>/Add
        [HttpPost("Add")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Add([FromBody] EventModel _event)
        {
            try
            {
                await eventService.AddAsync(_event);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/<EventsController>/Update/5
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update(int id, [FromBody] EventModel _event)
        {
            try
            {
                _event.Id = id;
                await eventService.UpdateAsync(_event);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/<EventsController>/5
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await eventService.DeleteAsync(id);
            return Ok();
        }

        // GET: api/<EventsController>
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<EventSubjectCategoryModel>>> GetCategories()
        {
            var events = await eventService.GetAllCategoriesAsync();
            return Ok(events);
        }

        // POST api/<EventsController>/Categories/Add
        [HttpPost("Categories/Add")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddCategory([FromBody] EventSubjectCategoryModel category)
        {
            try
            {
                await eventService.AddCategoryAsync(category);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/<EventsController>/Update/5
        [HttpPut("Categories/Update/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] EventSubjectCategoryModel category)
        {
            try
            {
                category.Id = id;
                await eventService.UpdateCategoryAsync(category);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/<EventsController>/5
        [HttpDelete("Categories/Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await eventService.DeleteCategoryAsync(id);
            return Ok();
        }
    }
}
