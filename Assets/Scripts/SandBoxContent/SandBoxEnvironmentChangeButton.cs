using Enums;
using UI.Buttons;
using UnityEngine;

namespace SandBoxContent
{
   public class SandBoxEnvironmentChangeButton : AbstractButton
   {
      [SerializeField] private Environments _environmentName;
      [SerializeField]private EnvironmentBuilder _environmentBuilder;
   
      protected override void OnClick()
      {
         _environmentBuilder.ChangeEnvironment(_environmentName);
      }
   }
}
