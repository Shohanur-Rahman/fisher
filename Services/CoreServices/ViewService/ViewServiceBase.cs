using System.Text.RegularExpressions;
using Core.ViewService.Enums;
using Core.ViewService.Models;

namespace Core.ViewService
{
    public class ViewServiceBase<T>
        where T : BaseEditorModel, new()
    {
        public ViewServiceBase()
        {
            _domainName = ToSentenceCase(typeof (T).Name);
        }

        public static string ToSentenceCase(string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
        }

        private readonly string _domainName;

        public virtual T GetViewModelForCreateGet(string displayName=null)
        {
            string title = displayName ?? _domainName;
            return new T
            {
                Title = "Create " + title,
                Description = string.Format("Create a {0}.", _domainName),
                EditorAction = EditorAction.Create
            };
        }

        public virtual T GetViewModelForEditGet(T viewModel, bool redirected, string displayName=null)
        {
            string title = displayName ?? _domainName;
            viewModel.Title = "Edit " + title;
            viewModel.UserActionResultMessage = !redirected
                ? ""
                : string.Format("{0} created successfully.", _domainName);
            viewModel.EditorAction = EditorAction.Edit;

            return viewModel;
        }

        public virtual T GetViewModelForEditPost(string id, T viewModel, string displayName = null)
        {
            string title = displayName ?? _domainName;
            viewModel.Title = "Edit " + title;
            viewModel.EditorAction = EditorAction.Edit;
            viewModel.UserActionResultMessage = string.Format("{0} updated successfully.", _domainName);
            return viewModel;
        }

        public virtual BaseEditorModel GetViewModelForIndex()
        {
            return new BaseEditorModel
            {
                Title = "Manage " + _domainName,
                Description = string.Format("Manage {0}.", _domainName)
            };

        }

        public virtual T GetViewModelForDetailsView(T viewModel, string displayName=null)
        {
            string title = displayName ?? _domainName;
            viewModel.Title = "View " + title;
            viewModel.EditorAction = EditorAction.View;
            return viewModel;
        }

    }
}