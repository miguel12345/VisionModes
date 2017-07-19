using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiguelFerreira;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CombinedVisionModes : MonoBehaviour
{

	public Canvas ThumbnailContainerCanvas;
	public RawImage NightVisionThumbnailRawImage;
	public RawImage ThermalVisionThumbnailRawImage;
	public RawImage EMVisionThumbnailRawImage;

	private NightVisionMode _nightVisionMode;
	private ThermalVisionMode _thermalVisionMode;
	private EMVisionMode _emVisionMode;

	private int _individualRenderTextureWidth;
	private int _individualRenderTextureHeight;

	private RenderTexture[] _renderTextures;
	private Camera _ownCamera;

	private void Awake()
	{
		_ownCamera = GetComponent<Camera>();

		_nightVisionMode = GetComponent<NightVisionMode>();
		_thermalVisionMode = GetComponent<ThermalVisionMode>();
		_emVisionMode = GetComponent<EMVisionMode>();
		
		_individualRenderTextureWidth = Mathf.CeilToInt((float)Screen.width / 4f);
		_individualRenderTextureHeight = _individualRenderTextureWidth * Mathf.CeilToInt(1f / _ownCamera.aspect);

		_renderTextures = Enumerable.Range(0, 3)
			.Select(_ => CreateSubEffectRenderTexture(_individualRenderTextureWidth, _individualRenderTextureHeight)).ToArray();

		NightVisionThumbnailRawImage.texture = _renderTextures[0];
		ThermalVisionThumbnailRawImage.texture = _renderTextures[1];
		EMVisionThumbnailRawImage.texture = _renderTextures[2];
		
		NightVisionThumbnailRawImage.gameObject.SetActive(false);
		ThermalVisionThumbnailRawImage.gameObject.SetActive(false);
		EMVisionThumbnailRawImage.gameObject.SetActive(false);

	}

	RenderTexture CreateSubEffectRenderTexture(int width,int height)
	{
		return new RenderTexture(width,height,24,RenderTextureFormat.ARGB32);
	}

	void UpdateRawImages(int renderTextureWidth,int renderTextureHeight)
	{
		UpdateRawImageTransform(NightVisionThumbnailRawImage.GetComponent<RectTransform>(),renderTextureWidth,renderTextureHeight,-1);
		UpdateRawImageTransform(ThermalVisionThumbnailRawImage.GetComponent<RectTransform>(),renderTextureWidth,renderTextureHeight,0);
		UpdateRawImageTransform(EMVisionThumbnailRawImage.GetComponent<RectTransform>(),renderTextureWidth,renderTextureHeight,1);
	}
	
	void UpdateRawImageTransform(RectTransform transform, int renderTextureWidth,int renderTextureHeight, int index)
	{
		transform.sizeDelta = new Vector2(renderTextureWidth,renderTextureHeight);
		transform.anchoredPosition = new Vector2((renderTextureWidth+20)*index,20);
	}

	private void OnEnable()
	{
		ThumbnailContainerCanvas.gameObject.SetActive(true);
		ActivateRawImages();
		UpdateRawImages(_individualRenderTextureWidth, _individualRenderTextureHeight);
		Update();
	}

	void ActivateRawImages()
	{
		NightVisionThumbnailRawImage.gameObject.SetActive(true);
		ThermalVisionThumbnailRawImage.gameObject.SetActive(true);
		EMVisionThumbnailRawImage.gameObject.SetActive(true);
		
	}

	private void Update()
	{
		RenderEffect(_nightVisionMode, _renderTextures[0]);
		RenderEffect(_thermalVisionMode, _renderTextures[1]);
		RenderEffect(_emVisionMode, _renderTextures[2]);
	}

	void RenderEffect(MonoBehaviour effectBehaviour, RenderTexture targetRenderTexture)
	{
		effectBehaviour.enabled = true;
		_ownCamera.targetTexture = targetRenderTexture;
		_ownCamera.forceIntoRenderTexture = true;
		_ownCamera.Render();
		_ownCamera.forceIntoRenderTexture = false;
		_ownCamera.targetTexture = null;
		effectBehaviour.enabled = false;
	}

	private void OnDisable()
	{
		if (ThumbnailContainerCanvas == null) return; //prevent unity editor exit NullReferenceException
		
		ThumbnailContainerCanvas.gameObject.SetActive(false);
		
		NightVisionThumbnailRawImage.gameObject.SetActive(false);
		ThermalVisionThumbnailRawImage.gameObject.SetActive(false);
		EMVisionThumbnailRawImage.gameObject.SetActive(false);
		
	}
}
