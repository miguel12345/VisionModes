using System.Collections;
using System.Collections.Generic;
using MiguelFerreira;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
[DefaultExecutionOrder(-20)] //prevent nullrefence exceptions when exiting unity editor in the OnDisable() method
public class CombinedVisionModes : MonoBehaviour
{

	public Camera NightVisionSubEffectCamera;
	public Camera ThermalVisionSubEffectCamera;
	public Camera EMVisionSubEffectCamera;

	public Canvas ThumbnailContainerCanvas;
	public RawImage NightVisionThumbnailRawImage;
	public RawImage ThermalVisionThumbnailRawImage;
	public RawImage EMVisionThumbnailRawImage;

	private int _individualRenderTextureWidth;
	private int _individualRenderTexturHeight;

	private void Awake()
	{
		var ownCamera = GetComponent<Camera>();
		
		_individualRenderTextureWidth = Mathf.CeilToInt((float)Screen.width / 4f);
		_individualRenderTexturHeight = _individualRenderTextureWidth * Mathf.CeilToInt(1f / ownCamera.aspect);

		NightVisionSubEffectCamera.targetTexture =
			CreateSubEffectRenderTexture(_individualRenderTextureWidth, _individualRenderTexturHeight);
		ThermalVisionSubEffectCamera.targetTexture =
			CreateSubEffectRenderTexture(_individualRenderTextureWidth, _individualRenderTexturHeight);
		EMVisionSubEffectCamera.targetTexture =
			CreateSubEffectRenderTexture(_individualRenderTextureWidth, _individualRenderTexturHeight);
		
		NightVisionSubEffectCamera.gameObject.SetActive(false);
		ThermalVisionSubEffectCamera.gameObject.SetActive(false);
		EMVisionSubEffectCamera.gameObject.SetActive(false);
		
		
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
		NightVisionThumbnailRawImage.texture = NightVisionSubEffectCamera.targetTexture;
		ThermalVisionThumbnailRawImage.texture = ThermalVisionSubEffectCamera.targetTexture;
		EMVisionThumbnailRawImage.texture = EMVisionSubEffectCamera.targetTexture;

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
		NightVisionSubEffectCamera.gameObject.SetActive(true);
		ThermalVisionSubEffectCamera.gameObject.SetActive(true);
		EMVisionSubEffectCamera.gameObject.SetActive(true);
		
		
		NightVisionThumbnailRawImage.gameObject.SetActive(true);
		ThermalVisionThumbnailRawImage.gameObject.SetActive(true);
		EMVisionThumbnailRawImage.gameObject.SetActive(true);
		
		ThumbnailContainerCanvas.gameObject.SetActive(true);
		
		UpdateRawImages(_individualRenderTextureWidth, _individualRenderTexturHeight);
	}
	
	private void OnDisable()
	{
		NightVisionSubEffectCamera.gameObject.SetActive(false);
		ThermalVisionSubEffectCamera.gameObject.SetActive(false);
		EMVisionSubEffectCamera.gameObject.SetActive(false);
		
		ThumbnailContainerCanvas.gameObject.SetActive(false);
		
		NightVisionThumbnailRawImage.gameObject.SetActive(false);
		ThermalVisionThumbnailRawImage.gameObject.SetActive(false);
		EMVisionThumbnailRawImage.gameObject.SetActive(false);
		
	}
}
