using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Landscaper.UI
{
	public class Tween : MonoBehaviour
	{
		public static Tween Instance;

		public AnimationCurve Liner;

		public AnimationCurve EaseInSine;
		public AnimationCurve EaseOutSine;

		public AnimationCurve EaseInQuart;
		public AnimationCurve EaseOutQuart;

		public AnimationCurve EaseInBounce;
		public AnimationCurve EaseOutBounce;

		public AnimationCurve EaseInElastic;
		public AnimationCurve EaseOutElastic;

		void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public IEnumerator coEase_Transform(Transform target, AnimationCurve curve,
			Vector3 axis, float start, float end, float duration, float delay, Action action = null)
		{
			if (axis == new Vector3(1, 0, 0))
				target.localPosition = new Vector3(Mathf.LerpUnclamped(start, end, 0), target.localPosition.y, target.localPosition.z);

			if (axis == new Vector3(0, 1, 0))
				target.localPosition = new Vector3(target.localPosition.x, Mathf.LerpUnclamped(start, end, 0), target.localPosition.z);

			if (axis == new Vector3(0, 0, 1))
				target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y, Mathf.LerpUnclamped(start, end, 0));

			yield return null;

			// Wait for the delay
			yield return new WaitForSecondsRealtime(delay);

			float counter = 0.0f;
			while (counter < duration && target)
			{
				// Add to the timer so we can calculate what percent of the animation we are through
				counter += Time.deltaTime;

				float value = curve.Evaluate(counter / duration);

				if (axis == new Vector3(1, 0, 0))
					target.localPosition = new Vector3(Mathf.LerpUnclamped(start, end, value), target.localPosition.y, target.localPosition.z);

				if (axis == new Vector3(0, 1, 0))
					target.localPosition = new Vector3(target.localPosition.x, Mathf.LerpUnclamped(start, end, value), target.localPosition.z);

				if (axis == new Vector3(0, 0, 1))
					target.localPosition = new Vector3(target.localPosition.x, target.localPosition.y, Mathf.LerpUnclamped(start, end, value));

				yield return null;
			}

			if (action != null) action.Invoke();
		}

		IEnumerator coEase_Rotation(Transform target, AnimationCurve curve,
			Vector3 axis, float start, float end, float duration, float delay, Action action = null)
		{
			// Wait for the delay
			yield return new WaitForSecondsRealtime(delay);

			float counter = 0.0f;
			while (counter < duration && target)
			{
				// Add to the timer so we can calculate what percent of the animation we are through
				counter += Time.deltaTime;

				float value = curve.Evaluate(counter / duration);

				if (axis == new Vector3(1, 0, 0))
					target.eulerAngles = new Vector3(Mathf.LerpUnclamped(start, end, value), target.eulerAngles.y, target.eulerAngles.z);

				if (axis == new Vector3(0, 1, 0))
					target.eulerAngles = new Vector3(target.eulerAngles.x, Mathf.LerpUnclamped(start, end, value), target.eulerAngles.z);

				if (axis == new Vector3(0, 0, 1))
					target.eulerAngles = new Vector3(target.eulerAngles.x, target.eulerAngles.y, Mathf.LerpUnclamped(start, end, value));

				yield return null;
			}

			if (action != null) action.Invoke();
		}

		IEnumerator coEase_Scale(Transform target, AnimationCurve curve,
			Vector3 axis, float start, float end, float duration, float delay, Action action = null)
		{
			if (axis.x == 1)
				target.localScale = new Vector3(Mathf.LerpUnclamped(start, end, 0), target.localScale.y, target.localScale.z);

			if (axis.y == 1)
				target.localScale = new Vector3(target.localScale.x, Mathf.LerpUnclamped(start, end, 0), target.localScale.z);

			if (axis.z == 1)
				target.localScale = new Vector3(target.localScale.x, target.localScale.y, Mathf.LerpUnclamped(start, end, 0));

			yield return null;

			// Wait for the delay
			yield return new WaitForSecondsRealtime(delay);

			float counter = 0.0f;
			while (counter < duration && target)
			{
				// Add to the timer so we can calculate what percent of the animation we are through
				counter += Time.deltaTime;

				float value = curve.Evaluate(counter / duration);

				if (axis.x == 1)
					target.localScale = new Vector3(Mathf.LerpUnclamped(start, end, value), target.localScale.y, target.localScale.z);

				if (axis.y == 1)
					target.localScale = new Vector3(target.localScale.x, Mathf.LerpUnclamped(start, end, value), target.localScale.z);

				if (axis.z == 1)
					target.localScale = new Vector3(target.localScale.x, target.localScale.y, Mathf.LerpUnclamped(start, end, value));

				yield return null;
			}

			if (action != null) action.Invoke();
		}


		// Transform
		#region Transform
		public void Ease_Transform_LinerX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, Liner, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void Ease_Transform_LinerY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, Liner, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void Ease_Transform_LinerZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, Liner, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseIn_Transform_SineX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInSine, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Transform_SineY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInSine, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Transform_SineZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInSine, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseOut_Transform_SineX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutSine, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Transform_SineY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutSine, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Transform_SineZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutSine, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseIn_Transform_QuartX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInQuart, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Transform_QuartY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInQuart, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Transform_QuartZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInQuart, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseOut_Transform_QuartX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutQuart, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Transform_QuartY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutQuart, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Transform_QuartZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutQuart, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseIn_Transform_BounceX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInBounce, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Transform_BounceY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInBounce, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Transform_BounceZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInBounce, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseOut_Transform_BounceX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutBounce, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Transform_BounceY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutBounce, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Transform_BounceZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutBounce, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseIn_Transform_ElasticX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInElastic, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Transform_ElasticY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInElastic, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Transform_ElasticZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseInElastic, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseOut_Transform_ElasticX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutElastic, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Transform_ElasticY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutElastic, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Transform_ElasticZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Transform(target, EaseOutElastic, new Vector3(0, 0, 1), start, end, duration, delay, action));
		#endregion

		// Scale
		#region Scale
		public void Ease_Scale_LinerX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, Liner, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void Ease_Scale_LinerY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, Liner, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void Ease_Scale_LinerZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, Liner, new Vector3(0, 0, 1), start, end, duration, delay, action));
		public void Ease_Scale_LinerAll(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, Liner, new Vector3(1, 1, 1), start, end, duration, delay, action));

		public void EaseOut_Scale_SineX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutSine, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Scale_SineY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutSine, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Scale_SineZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutSine, new Vector3(0, 0, 1), start, end, duration, delay, action));
		public void EaseOut_Scale_SineAll(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutSine, new Vector3(1, 1, 1), start, end, duration, delay, action));

		public void EaseIn_Scale_QuartX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInQuart, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Scale_QuartY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInQuart, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Scale_QuartZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInQuart, new Vector3(0, 0, 1), start, end, duration, delay, action));
		public void EaseIn_Scale_QuartAll(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInQuart, new Vector3(1, 1, 1), start, end, duration, delay, action));

		public void EaseOut_Scale_QuartX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutQuart, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Scale_QuartY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutQuart, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Scale_QuartZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutQuart, new Vector3(0, 0, 1), start, end, duration, delay, action));
		public void EaseOut_Scale_QuartAll(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutQuart, new Vector3(1, 1, 1), start, end, duration, delay, action));

		public void EaseIn_Scale_BounceX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInBounce, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Scale_BounceY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInBounce, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Scale_BounceZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInBounce, new Vector3(0, 0, 1), start, end, duration, delay, action));
		public void EaseIn_Scale_BounceAll(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInBounce, new Vector3(1, 1, 1), start, end, duration, delay, action));

		public void EaseOut_Scale_BounceX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutBounce, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Scale_BounceY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutBounce, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Scale_BounceZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutBounce, new Vector3(0, 0, 1), start, end, duration, delay, action));
		public void EaseOut_Scale_BounceAll(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutBounce, new Vector3(1, 1, 1), start, end, duration, delay, action));

		public void EaseIn_Scale_ElasticX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInElastic, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Scale_ElasticY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInElastic, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Scale_ElasticZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInElastic, new Vector3(0, 0, 1), start, end, duration, delay, action));
		public void EaseIn_Scale_ElasticAll(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseInElastic, new Vector3(1, 1, 1), start, end, duration, delay, action));

		public void EaseOut_Scale_ElasticX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutElastic, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Scale_ElasticY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutElastic, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Scale_ElasticZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutElastic, new Vector3(0, 0, 1), start, end, duration, delay, action));
		public void EaseOut_Scale_ElasticAll(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Scale(target, EaseOutElastic, new Vector3(1, 1, 1), start, end, duration, delay, action));

		#endregion

		// Rotation
		#region Rotation
		public void Ease_Rotation_LinerX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, Liner, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void Ease_Rotation_LinerY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, Liner, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void Ease_Rotation_LinerZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, Liner, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseOut_Rotation_SineX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutSine, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Rotation_SineY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutSine, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Rotation_SineZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutSine, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseIn_Rotation_QuartX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInQuart, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Rotation_QuartY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInQuart, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Rotation_QuartZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInQuart, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseOut_Rotation_QuartX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutQuart, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Rotation_QuartY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutQuart, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Rotation_QuartZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutQuart, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseIn_Rotation_BounceX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInBounce, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Rotation_BounceY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInBounce, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Rotation_BounceZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInBounce, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseOut_Rotation_BounceX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutBounce, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Rotation_BounceY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutBounce, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Rotation_BounceZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutBounce, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseIn_Rotation_ElasticX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInElastic, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseIn_Rotation_ElasticY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInElastic, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseIn_Rotation_ElasticZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseInElastic, new Vector3(0, 0, 1), start, end, duration, delay, action));

		public void EaseOut_Rotation_ElasticX(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutElastic, new Vector3(1, 0, 0), start, end, duration, delay, action));
		public void EaseOut_Rotation_ElasticY(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutElastic, new Vector3(0, 1, 0), start, end, duration, delay, action));
		public void EaseOut_Rotation_ElasticZ(Transform target, float start, float end, float duration, float delay = 0, Action action = null) => StartCoroutine(coEase_Rotation(target, EaseOutElastic, new Vector3(0, 0, 1), start, end, duration, delay, action));
		#endregion
	}
}