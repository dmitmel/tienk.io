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

namespace Tienkio {
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

        [Space]
        public int value;
        public SwitchButtonValue[] values;
        public SwitchButtonEvent onValueChanged = new SwitchButtonEvent();

        protected override void Start() {
            UpdateGraphics();
        }

        void UpdateGraphics() {
            SwitchButtonValue selectedValue = values[value];
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
                value++;
                if (value >= values.Length) value = 0;
                UpdateGraphics();
                onValueChanged.Invoke(value);
            }
        }
    }
}
