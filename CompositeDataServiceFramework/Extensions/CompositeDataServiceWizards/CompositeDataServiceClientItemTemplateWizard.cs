using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;

namespace CompositeDataServiceWizards
{
  public class CompositeDataServiceClientItemTemplateWizard : IWizard
  {
    public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
    {
    }

    public void ProjectFinishedGenerating(EnvDTE.Project project)
    {
    }

    public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
    {
    }

    public void RunFinished()
    {
    }

    public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
    {
        CompositeDataServiceClientConfigurationForm config = new CompositeDataServiceClientConfigurationForm();
        config.ShowDialog();
    }

    public bool ShouldAddProjectItem(string filePath)
    {
      return false;
    }
  }
}
