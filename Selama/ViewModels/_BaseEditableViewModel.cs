using System.Web.Mvc;

namespace Selama.ViewModels
{
    public abstract class _BaseEditableViewModel
    {
        public abstract void ValidateModel(ModelStateDictionary ModelState);
    }
}