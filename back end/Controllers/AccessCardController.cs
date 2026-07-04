using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccessCardController : ControllerBase
    {

        private readonly IAccessCardService _accessCardService;

        public AccessCardController(IAccessCardService accessCardService)
        {
            _accessCardService = accessCardService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AccessCardDTO>> AddAccessCards(AccessCardDTO NewAccessCard)
        {
            var InstertedCard = await _accessCardService.AddAccessCardAsync(NewAccessCard);

            // Return the created department
            return CreatedAtAction(nameof(GetAccessCardInfobyId), new { Id = NewAccessCard.Id }, InstertedCard);
        }

        [HttpGet]
        public async Task<ActionResult<AccessCardDTO>> GetAllAccessCards(int pageNumber=1, int pageSize=10)
        {
            var cards = await _accessCardService.GetAllCardsAsync(pageNumber,pageSize);
            return Ok(cards);

        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<AccessCardDTO>> UpdateAccessCards(int id,AccessCardDTO UpdatedAcccessCard)
        {
            var updateCard = await _accessCardService.UpdateAccessAsync(id,UpdatedAcccessCard);

            if (updateCard == null)
                return NotFound($"Student with Id {id} was not found");

            return Ok(updateCard);
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AccessCardDTO>>DeleteAccessCard(int id)
        {
            var deleted = await _accessCardService.DeleteAccessAsync(id);

            if (!deleted)
                return NotFound($"accessCard with Id {id} was not found");


            return Ok($"accessCard with Id {id} deleted successfully");


        }
        [HttpGet("{id:int}")]

        public async Task<ActionResult<AccessCardDTO>> GetAccessCardInfobyId(int id)
        {
            if (id < 0)
                return BadRequest($"Id '{id}' is not valid. Id must be a positive number");

            var teacher = await _accessCardService.GetAccessCardByIdAsync(id);
            if (teacher == null)
                return NotFound($"Teacher with Id {id} was not found");

            return Ok(teacher);



        }













    }











}
