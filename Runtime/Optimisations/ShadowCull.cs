using Akela.Tools;
using System.Reflection;
using UnityEngine;
#if AKELA_URP
using UnityEngine.Rendering.Universal;
#else
using UnityEngine.Rendering;
#endif

namespace Akela.Optimisations
{
	[AddComponentMenu("Optimisation/Shadow Cull", 2)]
	[RequireComponent(typeof(Light))]
	public class ShadowCull : MonoBehaviour, ICullingEventReceiver
	{
#if AKELA_URP
		private static readonly FieldInfo _additionalLightsShadowResolutionTier = typeof(UniversalAdditionalLightData)
			.GetField("m_AdditionalLightsShadowResolutionTier", BindingFlags.Instance | BindingFlags.NonPublic);
#endif

		#region Component Fields
#if AKELA_URP
		[Tooltip("If d < x shadows will be high quality\nIf x <= d < y shadows will be medium quality\nIf y <= d < z shadows will be low quality\nIf z <= d shadows will be disabled")]
		[SerializeField] Vector3Int _qualityBandRange = new(1, 2, 3);

		[Tooltip("If d < x shadows will be high resolution\nIf x <= d < y shadows will be medium resolution\nIf y <= d shadows will be low resolution")]
		[SerializeField] Vector2Int _resolutionBandRange = new(1, 3);
#else
		[Tooltip("If d < x shadows will be very high quality\nIf x <= d < y shadows will be high quality\nIf y <= d < z shadows will be medium quality\nIf z <= d < w shadows will be low quality\nIf w <= d shadows will be disabled")]
		[LineUp("X", "Y", "Z", "W")]
		[SerializeField] Vector4Int _distanceBandRange = new(1, 2, 3, 4);
#endif
		#endregion

#if AKELA_URP
		private UniversalAdditionalLightData _additionalLightData;

		private int ShadowResolutionTier
		{
			get => (int)_additionalLightsShadowResolutionTier.GetValue(_additionalLightData); 
			set => _additionalLightsShadowResolutionTier.SetValue(_additionalLightData, value); 
		}
#endif
		private Light _light;
		private LightShadows _shadowType;
		private int _qualitySetting;
		private int _resolutionTierSetting;
		private bool _visible;
		private int _currentBand = -1;

		public void OnCullingElementInvisible()
		{
			_visible = false;
			_light.shadows = LightShadows.None;
		}

		public void OnCullingElementVisible()
		{
			_visible = true;

#if AKELA_URP
			if (_currentBand < _qualityBandRange.z)
				_light.shadows = _shadowType;
#else
			if (_currentBand < _distanceBandRange.w)
				_light.shadows = _shadowType;
#endif
		}

		public void OnDistanceBandChanges(int _, int newBand)
		{
			_currentBand = newBand;

			if (!_visible)
				return;

#if AKELA_URP
			if (_visible && newBand < _qualityBandRange.z)
				_light.shadows = _shadowType;

			// Quality
			if (newBand < _qualityBandRange.x)
				_additionalLightData.softShadowQuality = (SoftShadowQuality)(_qualitySetting < 3 ? 0 : 3);
			else if (newBand < _qualityBandRange.y)
				_additionalLightData.softShadowQuality = (SoftShadowQuality)(_qualitySetting < 2 ? 0 : 2);
			else if (newBand < _qualityBandRange.z)
				_additionalLightData.softShadowQuality = SoftShadowQuality.Low;
			else
				_light.shadows = LightShadows.None;

			// Resolution
			if (newBand < _resolutionBandRange.x)
				ShadowResolutionTier = Mathf.Min(_resolutionTierSetting, 2);
			else if (newBand < _resolutionBandRange.y)
				ShadowResolutionTier = Mathf.Min(_resolutionTierSetting, 1);
			else
				ShadowResolutionTier = 0;
#else
			if (_visible && newBand < _distanceBandRange.w)
				_light.shadows = _shadowType;

			if (newBand < _distanceBandRange.x)
				_light.shadowResolution = (LightShadowResolution)(_qualitySetting < 3 ? -1 : 3);
			else if (newBand < _distanceBandRange.y)
				_light.shadowResolution = (LightShadowResolution)(_qualitySetting < 2 ? -1 : 2);
			else if (newBand < _distanceBandRange.z)
				_light.shadowResolution = (LightShadowResolution)(_qualitySetting < 1 ? -1 : 1);
			else if (newBand < _distanceBandRange.w)
				_light.shadowResolution = LightShadowResolution.Low;
			else
				_light.shadows = LightShadows.None;
#endif
		}

		#region Component Messages
		private void Awake()
		{
			_light = GetComponent<Light>();
			_shadowType = _light.shadows;

#if AKELA_URP
			_additionalLightData = GetComponent<UniversalAdditionalLightData>();

			_qualitySetting = (int)UrpGraphics.SoftShadowQuality;
			_resolutionTierSetting = ShadowResolutionTier;
#else
			_qualitySetting = (int)QualitySettings.shadowResolution;
#endif
		}
		#endregion
	}
}