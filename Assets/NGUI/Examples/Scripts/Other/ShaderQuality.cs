using UnityEngine;

/// <summary>
/// Change the shader level-of-detail to match the quality settings.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Examples/Shader Quality")]
public class ShaderQuality : MonoBehaviour
{
	int mCurrent = 600;

	void Update ()
	{
		int current = (QualitySettings.GetQualityLevel() + 1) * 100;

		if (mCurrent != current)
		{
			mCurrent = current;
			Shader.globalMaximumLOD = mCurrent;
		}
	}
}
