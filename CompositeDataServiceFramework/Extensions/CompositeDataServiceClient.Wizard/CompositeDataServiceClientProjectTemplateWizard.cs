﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;

namespace CompositeDataServiceClientWizards
{
  public class CompositeDataServiceClientProjectTemplateWizard : IWizard
  {
    public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
    {
      //  TODO: Not needed.
    }

    public void ProjectFinishedGenerating(EnvDTE.Project project)
    {
      //  TODO: Not needed.
    }

    public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
    {
      //  TODO: Not needed.
    }

    public void RunFinished()
    {
      //  TODO: Not needed.
    }

    public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
    {
      System.Windows.Forms.MessageBox.Show("Wizard");
      //  TODO: Not needed.
    }

    public bool ShouldAddProjectItem(string filePath)
    {
      //  TODO: Not needed.
      return false;
    }
  }
}
