using Akela.Motion;
using AkelaEditor.Tools;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace AkelaEditor.Motion
{
    [Overlay(typeof(SceneView), "Transform Shift")]
    public class TransformShiftOverlay : ComponentOverlay<TransformShift>
    {
        private Toggle _previewToggle;
        private Button _playButton;
        private Button _pauseButton;
        private Button _stopButton;
        private Slider _slider;
        private AnimationModeDriver _transformPropertyDriver;

        protected override void OnBecomeInactive()
        {
            EditorApplication.update -= UpdateSliderState;

            if (_transformPropertyDriver)
            {
                AnimationMode.StopAnimationMode(_transformPropertyDriver);

                DrivenPropertyManager.UnregisterProperties(_transformPropertyDriver);
                Object.DestroyImmediate(_transformPropertyDriver);
                _transformPropertyDriver = null;
            }

            target.Stop();
            target.SetPositionAtStart();

            target.ControlledByEditor = false;
        }

        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement();

            _previewToggle = new Toggle("Preview");
            _previewToggle.RegisterValueChangedCallback(e => SetPreviewEnabled(e.newValue));

            root.Add(_previewToggle);

            var buttons = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row
                }
            };

            _playButton = new Button(Play) { text = "Play", style = { width = 48f } };
            _pauseButton = new Button(Pause) { text = "Pause", style = { width = 48f } };
            _stopButton = new Button(Stop) { text = "Stop", style = { width = 48f } };

            UpdateButtonState();

            buttons.Add(_playButton);
            buttons.Add(_pauseButton);
            buttons.Add(_stopButton);

            root.Add(buttons);

            _slider = new Slider(0f, 1f) { enabledSelf = false };

            UpdateSliderState();

            root.Add(_slider);

            return root;
        }

        private void ResetAnimationDriver()
        {
            if (_transformPropertyDriver)
            {
                AnimationMode.StopAnimationMode(_transformPropertyDriver);
                DrivenPropertyManager.UnregisterProperties(_transformPropertyDriver);
                Object.DestroyImmediate(_transformPropertyDriver);
                
                _transformPropertyDriver = null;
            }
            
            _transformPropertyDriver = ScriptableObject.CreateInstance<AnimationModeDriver>();

            DrivenPropertyManager.RegisterProperty(_transformPropertyDriver, target.transform, "m_LocalPosition");
            DrivenPropertyManager.RegisterProperty(_transformPropertyDriver, target.transform, "m_LocalRotation");
            DrivenPropertyManager.RegisterProperty(_transformPropertyDriver, target.transform, "m_LocalScale");
        }
        
        private void SetPreviewEnabled(bool enabled)
        {
            if (enabled)
            {
                ResetAnimationDriver();
                
                AnimationMode.StartAnimationMode(_transformPropertyDriver);
                target.ControlledByEditor = true;

                EditorApplication.update += UpdateSliderState;
            }
            else
            {
                target.Stop();
                target.SetPositionAtStart();
                
                AnimationMode.StopAnimationMode(_transformPropertyDriver);
                DrivenPropertyManager.UnregisterProperties(_transformPropertyDriver);
                Object.DestroyImmediate(_transformPropertyDriver);
                _transformPropertyDriver = null;
                
                target.ControlledByEditor = false;
                
                EditorApplication.update -= UpdateSliderState;
                UpdateSliderState();
            }

            UpdateButtonState();
        }

        private void Play()
        {
            if (!_previewToggle.value)
                return;
            
            target.ForceResetEditorDeltaTime();
            target.Play();
            
            UpdateButtonState();
        }

        private void Pause()
        {
            if (!_previewToggle.value)
                return;
            
            target.Pause();

            UpdateButtonState();
        }

        private void Stop()
        {
            if (!_previewToggle.value)
                return;
            
            target.Stop();

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            _playButton.SetEnabled(_previewToggle.value && target.PlayingState != TransformAnimationPlayingState.Playing);
            _pauseButton.SetEnabled(_previewToggle.value && target.PlayingState == TransformAnimationPlayingState.Playing);
            _stopButton.SetEnabled(_previewToggle.value && target.PlayingState != TransformAnimationPlayingState.Stopped);
        }

        private void UpdateSliderState()
        {
            if (_slider == null || !target)
            {
                EditorApplication.update -= UpdateSliderState;
                return;
            }

            _slider.SetValueWithoutNotify(target.Progression);
        }
    }
}
