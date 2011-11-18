using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;

namespace CompositeDataServiceClientWizards
{
  public class CompositeDataServiceClientItemTemplateWizard : IWizard
  {
    public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
    {
      System.Windows.Forms.MessageBox.Show("BeforeOpeningFile");
    }

    public void ProjectFinishedGenerating(EnvDTE.Project project)
    {
      System.Windows.Forms.MessageBox.Show("ProjectFinishedGenerating");
    }

    public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
    {
      System.Windows.Forms.MessageBox.Show("ProjectItemFinishedGenerating");
    }

    public void RunFinished()
    {
      System.Windows.Forms.MessageBox.Show("RunFinished");
    }

    public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
    {
      System.Windows.Forms.MessageBox.Show("RunStarted");
      //  TODO: Not needed.
    }

    public bool ShouldAddProjectItem(string filePath)
    {
      System.Windows.Forms.MessageBox.Show("ShouldAddProjectItem");
      return false;
    }
  }
}
