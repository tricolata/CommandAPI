// using Directives
using System.Collections.Generic;   // supports IEnumerable
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;     // everything else 

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")] // decorater
    [ApiController]             // automatic HTTP error responses and other routing uses
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repository;
        public CommandsController(ICommandAPIRepo repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands ()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(commandItems);
        }

        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null) return NotFound();
            return Ok(commandItem);
        }
    }
}


/*
    - ControllerBase
        - ControllerBase has MVC support except for V
        - Controller has full MVC support
        - more info is at microsoft docs on ControllerBase

*/