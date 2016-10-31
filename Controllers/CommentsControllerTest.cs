using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.BLL.DTO;
using GameStore.BLL.Infrastructure;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Controllers;
using GameStore.WEB.ViewModels;
using Moq;
using NLog;
using NUnit.Framework;

namespace GameStore.WEB.Tests.Controllers
{
    [TestFixture]
    public class CommentsControllerTest
    {
        private readonly Mock<ILogger> _loggerMock;

        public CommentsControllerTest()
        {
            _loggerMock = new Mock<ILogger>();
        }

        #region GetGameComments

        [Test]
        public void GetGameComments_GetsValidGameKey_ReturnsCommentsJson()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.GetCommentsByGame(It.IsAny<string>())).Returns(new List<CommentDTO> {new CommentDTO(), new CommentDTO()});
            var sut = new CommentsController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGameComments("valid game key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.TypeOf(typeof(List<CommentViewModel>)));
            Assert.That(res.Data as List<CommentViewModel>, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetGameComments_GetsInvalidGameKey_ReturnsEmptyJson()
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.GetCommentsByGame(It.IsAny<string>())).Returns(new List<CommentDTO>());
            var sut = new CommentsController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.GetGameComments("invalid game key");

            // Assert
            Assert.That(res, Is.TypeOf(typeof(JsonResult)));
            Assert.That(res.Data, Is.Empty);
        }

        #endregion

        #region CreateComment

        [Test, TestCaseSource(typeof(CommentViewModelDataClass), nameof(CommentViewModelDataClass.CommentValid))]
        public void CreateComment_GetsValidData_ReturnsStatusCodeOk(CommentViewModel commentView, string gamekey)
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.CreateComment(It.IsAny<CommentDTO>(), gamekey)).Verifiable();
            var sut = new CommentsController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.CreateComment(commentView, gamekey);

            // Assert            
            Assert.That(res.StatusCode, Is.EqualTo(201));
            Mock.Verify(mock);
        }

        [Test, TestCaseSource(typeof(CommentViewModelDataClass), nameof(CommentViewModelDataClass.CommentInvalid))]
        public void CreateComment_GetsInvalidData_ReturnsStatusCodeBadRequest(CommentViewModel commentView, string gamekey)
        {
            // Arrange
            var mock = new Mock<IStoreService>();
            mock.Setup(a => a.CreateComment(It.IsAny<CommentDTO>(), gamekey)).Throws( new ValidationException("", "")).Verifiable();
            var sut = new CommentsController(mock.Object, _loggerMock.Object);

            // Act
            var res = sut.CreateComment(commentView, gamekey);

            // Assert            
            Assert.That(res.StatusCode, Is.EqualTo(400));
            Mock.Verify(mock);
        }

        #endregion
    }
}
