using UnityEngine;
using TMPro;

namespace Presentation.Shared.Views
{
    public class EditableTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textField;

        public void SetText<T>(T value)
        {
            _textField.text = value.ToString();
        }
    }
}