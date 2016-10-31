using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Controllers;
using GameStore.WEB.ViewModels;
using Moq;
using NUnit.Framework;
using NLog;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class GamesControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;

        public GamesControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
        }

        #region Get

        [Test]
        public void GetGames_BLLReturnsSomeData_ReturnsGamesJson()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.GetGames()).Returns(new List<GameDTO> { new GameDTO(), new GameDTO() });
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Get();

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(List<GameViewModel>)));
            Assert.That(res.Data as List<GameViewModel>, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetGames_BLLReturnsNothing_ReturnsEmptyJson()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.GetGames()).Returns(new List<GameDTO>());
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Get();

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Empty);
        }

        #endregion

        #region GetDetails

        [Test]
        public void GetGameDetails_GetsValidGameKey_ReturnsGameJson()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.GetGame(It.IsAny<string>())).Returns(new GameDTO());
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetDetails("valid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(GameViewModel)));
            Assert.That(res.Data as GameViewModel, Is.Not.Null);
        }

        [Test]
        public void GetGameDetails_GetsInvalidGameKey_ReturnsEmptyJson()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.GetGame(It.IsAny<string>())).Throws(new ValidationException("", ""));
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetDetails("invalid-key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Null.Or.Empty);
        }

        #endregion

        #region New

        [Test]
        public void CreateGame_GetsValidItem_ReturnsStatusCodeCreated()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.CreateGame(It.IsAny<GameDTO>())).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.New(new GameViewModel
            {
                Name = "valid-game",
                Genres = new List<GenreViewModel> {new GenreViewModel()},
                Platforms = new List<PlatformViewModel> {new PlatformViewModel()}
            });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public void CreateGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.CreateGame(It.IsAny<GameDTO>())).Throws(new ValidationException("", "")).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.New(new GameViewModel { Name = "invalid-game" });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(400));
        }

        #endregion     

        #region Update

        [Test]
        public void UpdateGame_GetsValidItem_ReturnsStatusCodeOk()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.UpdateGame(It.IsAny<GameDTO>())).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Update(new GameViewModel
            {
                Name = "valid-game",
                Genres = new List<GenreViewModel> { new GenreViewModel() },
                Platforms = new List<PlatformViewModel> { new PlatformViewModel() }
            });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void UpdateGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.UpdateGame(It.IsAny<GameDTO>())).Throws(new ValidationException("", "")).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Update(new GameViewModel { Name = "invalid-game" });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(400));
        }

        #endregion     

        #region Remove

        [Test]
        public void RemoveGame_GetsValidItem_ReturnsStatusCodeOk()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.DeleteGame(It.IsAny<int>())).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Remove(new GameViewModel
            {
                Name = "valid-game",
                Genres = new List<GenreViewModel> { new GenreViewModel() },
                Platforms = new List<PlatformViewModel> { new PlatformViewModel() }
            });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void RemoveGame_GetsInvalidItem_ReturnsStatusCodeBadRequest()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.DeleteGame(It.IsAny<int>())).Throws(new ValidationException("", "")).Verifiable();
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Remove(new GameViewModel { Name = "invalid-game" });

            // Assert
            Mock.Verify(mock);
            Assert.That(res.StatusCode, Is.EqualTo(400));
        }

        #endregion

        #region Download

        [Test]
        public void DownloadGame_GetsInvalidKey_ReturnsNull()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.GetGame(It.IsAny<string>())).Throws(new ValidationException("", ""));
            var sut = new GamesController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.Download("invalid-key");

            // Assert
            Assert.That(res, Is.Null);
        }

        #endregion
    }
}

