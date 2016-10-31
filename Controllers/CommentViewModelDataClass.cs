using System.Collections;
using GameStore.WEB.ViewModels;
using NUnit.Framework;

namespace GameStore.WEB.Tests.Controllers
{
    public class CommentViewModelDataClass
    {
        public static IEnumerable CommentValid
        {
            get
            {
                yield return new TestCaseData(new CommentViewModel { Name = "stub-name", Body = "stub-body", GameId = 1 }, "stub-gamekey");
            }
        }

        public static IEnumerable CommentInvalid
        {
            get
            {
                yield return new TestCaseData(new CommentViewModel { Name = null, Body = null, GameId = 1 }, "stub-gamekey");
            }
        }
    }
}
