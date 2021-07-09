using System;
using Xunit;
using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Controllers;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CommandAPI.Dtos;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;   

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            realProfile = null;
            configuration = null;
            mapper = null;
        }

        // Naming Convention : <method_name>_<expected_result>_<condition>
        // GetAllCommands()
        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBisEmpty()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));
            var controller = new CommandsController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetAllCommands();

            // Assert
            //Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetAllCommand_ReturnOneItem_WhenDBHasOneItem()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetAllCommands();

            // Assert
            var okResult = result.Result as OkObjectResult;

            var commands = okResult.Value as List<CommandReadDto>;
            Assert.Single(commands);
        }

        [Fact]
        public void GetAllCommand_Return200Ok_WhenDBHasOneItem()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetAllCommands();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommand_ReturnCorrectType_WhenDBHasOneItem()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetAllCommands();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        /*
        // GetCommandById
        public void GetCommandById_Return200Ok_ValidId()
        public void GetCommandById_Return404NotFound_InvalidID()
        public void GetCommandById_ReturnCorrectType_ValidId()

        // CreateCommand
        public void CreateCommand_ReturnObjectType_ValidObjectSubmitted()
        public void CreateCommand_Return201_ValidObjectSubmitted()

        // UpdateCommand
        public void UpdateCommand_Return204NoContent_ValidObjectSubmitted()
        public void UpdateCommand_Return404NotFound_NonexistentIdSubmitted()

        // PartialCommandUpdate
        public void PartialCommandUpdate_Return404NotFound_NonexistentIdSubmitted()

        // DeleteCommand
        public void DeleteCommand_Return204NoContent_ValidIdSubmitted()
        public void DeleteCommand_Return404NotFound_NonexistentIdSubmitted()
        */

        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "copy all files inside directory",
                    Platform = "Linux",
                    CommandLine = "cp . <destination>"
                });
            }
            return commands;
        }

    }
}

