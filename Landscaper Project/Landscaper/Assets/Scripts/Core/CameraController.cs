using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Landscaper.Core
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera _virtualCamera = null;
		[SerializeField] private Transform _virtualCamera_Target = null;
		[SerializeField] private Transform _virtualCamera_ZoomOut = null;

		public float CameraSpeed = 16f;
		public float CameraOffsetY = 7;
		public float CameraOffsetZ = -4;

		[Range(0,1f)]
		public float scroll = 0.5f;

		// Update is called once per frame
		void Update()
		{
			Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

			//if (Input.GetKey(KeyCode.Q)) _virtualCamera_Target.Rotate(0,  75.0f * Time.deltaTime, 0);
			//if (Input.GetKey(KeyCode.E)) _virtualCamera_Target.Rotate(0, -75.0f * Time.deltaTime, 0);

			// TODO: 90* Snaping

			if (Input.GetKeyDown(KeyCode.Mouse2))
			{
				_virtualCamera_ZoomOut.gameObject.SetActive(!_virtualCamera_ZoomOut.gameObject.activeSelf);
			}

			_virtualCamera_Target.transform.Translate(input * CameraSpeed * Time.deltaTime);


			scroll = Mathf.Clamp(scroll + (Input.mouseScrollDelta.y * 5f * Time.deltaTime), 0, 1);

			CameraOffsetZ = Mathf.Lerp(-2, -6, scroll);
			CameraOffsetY = Mathf.Lerp(12, 4, scroll);	

			var transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
			transposer.m_FollowOffset = new Vector3(0, CameraOffsetY, CameraOffsetZ);
		}
	}
}