using Akela.Globals;
using Akela.Optimisations;
using System;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Optimisations
{
    [CustomEditor(typeof(CullingSystem))]
    internal class CullingSystemEditor : Editor
    {
        private const int MAX_BAND_COUNT = 8;
        private const float DRAG_AREA_SIZE = 6f;
        private static readonly Color[] BAND_COLORS =
        {
            new(.4831376f, .6211768f, .0219608f, .6f),
            new(.2792160f, .4078432f, .5835296f, .6f),
            new(.2070592f, .5333336f, .6556864f, .6f),
            new(.5333336f, .1600000f, .0282352f, .6f),
            new(.3827448f, .2886272f, .5239216f, .6f),
            new(.8000000f, .4423528f, .0000000f, .6f),
            new(.4486272f, .4078432f, .0501960f, .6f),
            new(.7749016f, .6368624f, .0250984f, .6f)
        };

        private int _draggingId = -1;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "_distanceBands", "_maxiumCullingDistance");

            EditorGUILayout.Space(18f);

            var distanceValue = (Var<float>)serializedObject.FindProperty("_maxiumCullingDistance").boxedValue;
            var sliderBarPosition = GUILayoutUtility.GetRect(0f, 30f, GUILayout.ExpandWidth(true));
            DrawSlider(sliderBarPosition, serializedObject.FindProperty("_distanceBands"), distanceValue);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxiumCullingDistance"));

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSlider(Rect rect, SerializedProperty distanceBandsProperty, float maxDistance)
        {
            var evt = Event.current;
            var dragging = evt.type == EventType.MouseDrag;
            var leftClicking = evt.type == EventType.MouseDown && evt.button == 0;
            var leftReleasing = evt.type == EventType.MouseUp && evt.button == 0;
            var rightClicking = evt.type == EventType.MouseDown && evt.button == 1;

            // Outline
            var outlineRect = new Rect(rect);
            outlineRect.max -= Vector2.one;
            outlineRect.min += Vector2.one;

            EditorGUI.DrawRect(rect, Color.black);
            EditorGUI.DrawRect(outlineRect, new(.22f, .22f, .22f));

            // Bands
            var count = distanceBandsProperty.arraySize;
            var cumulatedOffset = 0f;
            var cumulatedPercent = 0f;

            for (var i = 0; i < count; ++i)
            {
                var bandValue = distanceBandsProperty.GetArrayElementAtIndex(i).floatValue;
                cumulatedPercent += bandValue;

                var bandRect = new Rect(rect);
                bandRect.width *= bandValue;
                bandRect.x += cumulatedOffset;

                cumulatedOffset += bandRect.width;

                var color = BAND_COLORS[i];

                EditorGUI.DrawRect(bandRect, color);
                EditorGUI.LabelField(bandRect, $"Band {i}\n{Math.Round(cumulatedPercent * maxDistance, 2)}", EditorStyles.whiteMiniLabel);

                // Allowing drag or not
                var allowDragging = false;
                var dragRect = new Rect(bandRect.xMax - DRAG_AREA_SIZE, bandRect.yMin, DRAG_AREA_SIZE * 2f, bandRect.height);

                if (i < count - 1)
                {
                    allowDragging = true;
                    EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.ResizeHorizontal);
                }

                // Interaction
                if (dragging && _draggingId == i)
                {
                    var percentDelta = evt.delta.x / rect.width;

                    ResizeBand(i, percentDelta, distanceBandsProperty);

                    Repaint();
                }
                else if (leftClicking && allowDragging && dragRect.Contains(evt.mousePosition))
                {
                    _draggingId = i;
                }
                else if (leftReleasing && _draggingId == i)
                {
                    _draggingId = -1;
                }
                else if (rightClicking && bandRect.Contains(evt.mousePosition))
                {
                    var menu = new GenericMenu();
                    var currentIndex = i;

                    if (count < MAX_BAND_COUNT)
                        menu.AddItem(new GUIContent("Insert"), false, () => InsertBand(currentIndex, (evt.mousePosition.x - bandRect.x) / bandRect.width, distanceBandsProperty));
                    else
                        menu.AddDisabledItem(new GUIContent("Insert"));

                    if (count > 1)
                        menu.AddItem(new GUIContent("Delete"), false, () => DeleteBand(currentIndex, distanceBandsProperty));
                    else
                        menu.AddDisabledItem(new GUIContent("Delete"));

                    menu.ShowAsContext();
                }
            }
        }

        private void InsertBand(int index, float percentOfCurrent, SerializedProperty distanceBandsProperty)
        {
            var originalValue = distanceBandsProperty.GetArrayElementAtIndex(index).floatValue;
            distanceBandsProperty.GetArrayElementAtIndex(index).floatValue = originalValue * (1f - percentOfCurrent);

            distanceBandsProperty.InsertArrayElementAtIndex(index);
            distanceBandsProperty.GetArrayElementAtIndex(index).floatValue = originalValue * percentOfCurrent;

            NormaliseBands(distanceBandsProperty);

            serializedObject.ApplyModifiedProperties();
        }

        private void DeleteBand(int index, SerializedProperty distanceBandsProperty)
        {
            var deletedValue = distanceBandsProperty.GetArrayElementAtIndex(index).floatValue;
            distanceBandsProperty.DeleteArrayElementAtIndex(index);

            var count = distanceBandsProperty.arraySize;
            var newIndex = index >= count ? count - 1 : index;

            distanceBandsProperty.GetArrayElementAtIndex(newIndex).floatValue += deletedValue;

            NormaliseBands(distanceBandsProperty);

            serializedObject.ApplyModifiedProperties();
        }

        private void ResizeBand(int index, float amount, SerializedProperty distanceBandsProperty)
        {
            var currentBand = distanceBandsProperty.GetArrayElementAtIndex(index);
            var currentBandValue = currentBand.floatValue;

            var nextBand = distanceBandsProperty.GetArrayElementAtIndex(index + 1);
            var nextBandValue = nextBand.floatValue;

            var upperLimit = currentBandValue + nextBandValue;

            currentBand.floatValue = Mathf.Clamp(currentBandValue + amount, .05f, upperLimit - .05f);
            nextBand.floatValue = Mathf.Clamp(nextBandValue - amount, .05f, upperLimit - .05f);

            NormaliseBands(distanceBandsProperty);

            serializedObject.ApplyModifiedProperties();
        }

        private void NormaliseBands(SerializedProperty distanceBandsProperty)
        {
            var count = distanceBandsProperty.arraySize;
            var cumulatedPercent = 0f;

            for (var i = 0; i < count - 1; ++i)
                cumulatedPercent += distanceBandsProperty.GetArrayElementAtIndex(i).floatValue;

            distanceBandsProperty.GetArrayElementAtIndex(count - 1).floatValue = 1f - cumulatedPercent;
        }
    }
}
