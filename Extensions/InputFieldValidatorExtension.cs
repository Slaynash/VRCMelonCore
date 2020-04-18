using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace VRCMelonCore.Extensions
{
    public static class InputFieldValidatorExtension
    {
        public static bool IsFormInputValid(GameObject go)
        {
			bool result = true;
			InputFieldValidator[] componentsInChildren = go.GetComponentsInChildren<InputFieldValidator>();
			foreach (InputFieldValidator inputFieldValidator in componentsInChildren)
			{
				if (inputFieldValidator.gameObject.activeInHierarchy && inputFieldValidator.enabled)
				{
					if (!inputFieldValidator.prop_Boolean_0)
					{
						inputFieldValidator.UpdateValidateTexture(inputFieldValidator.isInvalidTexture);
						result = false;
					}
					else
					{
						inputFieldValidator.UpdateValidateTexture(inputFieldValidator.isValidTexture);
					}
				}
			}
			return result;
		}



		private static void UpdateValidateTexture(this InputFieldValidator inputFieldValidator, Sprite texture)
		{
			inputFieldValidator.validateImage.type = Image.Type.Sliced;

			if (inputFieldValidator.validateImage != null)
			{
				Color color = inputFieldValidator.validateImage.color;
				color.a = 1f;
				inputFieldValidator.validateImage.color = color;
				inputFieldValidator.validateImage.sprite = texture;
			}
		}
    }
}
