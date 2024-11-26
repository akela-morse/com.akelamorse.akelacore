using UnityEngine;
#if AKELA_URP
using Akela.Tools;
using UnityEngine.Rendering.Universal;
using System.Reflection;
#else
using UnityEngine.Rendering;
#endif

namespace Akela.Optimisations
{
    [AddComponentMenu("Optimisation/Shadow Cull", 2)]
    [DisallowMultipleComponent, RequireComponent(typeof(Light))]
    public class ShadowCull : MonoBehaviour, ICullingMessageReceiver
    {
#if AKELA_URP
        private static readonly FieldInfo _additionalLightsShadowResolutionTier = typeof(UniversalAdditionalLightData)
            .GetField("m_AdditionalLightsShadowResolutionTier", BindingFlags.Instance | BindingFlags.NonPublic);
#endif

        #region Component Fields
#if AKELA_URP
        [Tooltip("If d < x shadows will be high quality\nIf x <= d < y shadows will be medium quality\nIf y <= d shadows will be low quality")]
        [SerializeField] Vector2Int _qualityBandRange = new(1, 2);

        [Tooltip("If d < x shadows will be high resolution\nIf x <= d < y shadows will be medium resolution\nIf y <= d shadows will be low resolution")]
        [SerializeField] Vector2Int _resolutionBandRange = new(2, 3);
#else
        [Tooltip("If d < x shadows will be very high quality\nIf x <= d < y shadows will be high quality\nIf y <= d < z shadows will be medium quality\nIf z <= d shadows will be low quality")]
        [SerializeField] Vector3Int _distanceBandRange = new(1, 2, 3);
#endif
        #endregion

#if AKELA_URP
        private UniversalAdditionalLightData _additionalLightData;
        private int _resolutionTierSetting;

        private int ShadowResolutionTier
        {
            get => (int)_additionalLightsShadowResolutionTier.GetValue(_additionalLightData);
            set => _additionalLightsShadowResolutionTier.SetValue(_additionalLightData, value);
        }
#else
        private Light _light;
#endif
        private int _qualitySetting;

        public void OnCullingElementInvisible() { }

        public void OnCullingElementVisible() { }

        public void OnDistanceBandChanges(int _, int newBand)
        {
#if AKELA_URP
            // Quality
            if (newBand < _qualityBandRange.x)
                _additionalLightData.softShadowQuality = (SoftShadowQuality)(_qualitySetting < 3 ? 0 : 3);
            else if (newBand < _qualityBandRange.y)
                _additionalLightData.softShadowQuality = (SoftShadowQuality)(_qualitySetting < 2 ? 0 : 2);
            else
                _additionalLightData.softShadowQuality = SoftShadowQuality.Low;

            // Resolution
            if (newBand < _resolutionBandRange.x)
                ShadowResolutionTier = Mathf.Min(_resolutionTierSetting, 2);
            else if (newBand < _resolutionBandRange.y)
                ShadowResolutionTier = Mathf.Min(_resolutionTierSetting, 1);
            else
                ShadowResolutionTier = 0;
#else
            if (newBand < _distanceBandRange.x)
                _light.shadowResolution = (LightShadowResolution)(_qualitySetting < 3 ? -1 : 3);
            else if (newBand < _distanceBandRange.y)
                _light.shadowResolution = (LightShadowResolution)(_qualitySetting < 2 ? -1 : 2);
            else if (newBand < _distanceBandRange.z)
                _light.shadowResolution = (LightShadowResolution)(_qualitySetting < 1 ? -1 : 1);
            else
                _light.shadowResolution = LightShadowResolution.Low;
#endif
        }

        #region Component Messages
        private void Awake()
        {
#if AKELA_URP
            _additionalLightData = GetComponent<UniversalAdditionalLightData>();

            _qualitySetting = (int)UrpGraphics.SoftShadowQuality;
            _resolutionTierSetting = ShadowResolutionTier;
#else
            _light = GetComponent<Light>();

            _qualitySetting = (int)QualitySettings.shadowResolution;
#endif
        }
        #endregion
    }
}