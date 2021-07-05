// using Directives
using System.Collections.Generic;   // supports IEnumerable
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;     // everything else 
using AutoMapper;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers
{
    // random change added again
    
    [Route("api/[controller]")] // decorater
    [ApiController]             // automatic HTTP error responses and other routing uses
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repository;
        private readonly IMapper _mapper;
        public CommandsController(ICommandAPIRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null) return NotFound();
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public ActionResult <CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            int id = _repository.CreateCommand(commandModel);

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
            commandReadDto.Id = id;
            return CreatedAtRoute(nameof(GetCommandById), new {id = id}, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            if (_repository.GetCommandById(id) == null) return NotFound();
            var commandToUpdate = _mapper.Map<Command>(commandUpdateDto);
            commandToUpdate.Id = id;
            
            _repository.UpdateCommand(commandToUpdate);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null) return NotFound();

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            // applies the patch to the command
            patchDoc.ApplyTo(commandToPatch, ModelState);
            // tryvalidatemodel is from ControllerBase and validate using model's state (annotation)
            if (!TryValidateModel(commandToPatch)) return ValidationProblem(ModelState);

            _mapper.Map(commandToPatch, commandModelFromRepo);
            _repository.UpdateCommand(commandModelFromRepo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null) return NotFound();

            _repository.DeleteCommand(commandModelFromRepo);
            return NoContent();
        }
    }
}


/*
    - ControllerBase
        - ControllerBase has MVC support except for V
        - Controller has full MVC support
        - more info is at microsoft docs on ControllerBase

*/