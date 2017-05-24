//
//  Copyright (c) 2017  FederationOfCoders.org
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tienkio.UI {
    public class SwitchButton : Selectable, IPointerClickHandler, ISubmitHandler, IEventSystemHandler {
        [System.Serializable]
        public class SwitchButtonEvent : UnityEvent<int> { }

        [System.Serializable]
        public class SwitchButtonValue {
            public string name;
            public Color color;
        }

        [Space]
        public Text valueText;

        [Space, SerializeField]
        int _value;
        public SwitchButtonValue[] values;
        public SwitchButtonEvent onValueChanged = new SwitchButtonEvent();

        public int value {
            get { return _value; }
            set {
                int maxValue = Mathf.Max(0, values.Length - 1);
                int clampedValue = Mathf.Clamp(value, 0, maxValue);
                if (clampedValue != _value) {
                    _value = clampedValue;
                    UpdateGraphics();
                    onValueChanged.Invoke(_value);
                }
            }
        }

        protected override void Start() {
            base.Start();
            UpdateGraphics();
        }

#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            value = _value;
        }
#endif

        void UpdateGraphics() {
            SwitchButtonValue selectedValue = values[_value];
            valueText.text = selectedValue.name;
            targetGraphic.color = selectedValue.color;
        }

        public virtual void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) SelectNextValue();
        }

        public virtual void OnSubmit(BaseEventData eventData) {
            SelectNextValue();
            if (IsActive() && IsInteractable()) {
                DoStateTransition(SelectionState.Pressed, false);
            }
        }

        void SelectNextValue() {
            if (IsActive() && IsInteractable()) {
                _value++;
                if (_value >= values.Length) _value = 0;
                UpdateGraphics();
                onValueChanged.Invoke(_value);
            }
        }
    }
}
