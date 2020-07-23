using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public static class Builder
{
	static string[] scenes = { "Assets/Remote.unity" };


	[MenuItem("Build/Build iOS")]
	public static void BuildiOS()
	{
		string error = BuildPipeline.BuildPlayer(scenes, "build/UnityRemoteNG-iOS", BuildTarget.iOS, BuildOptions.None).ToString();

		if (error != null && error.Length > 0) {
			throw new Exception("Build failed: " + error);
		}
	}


	[MenuItem("Build/Build Android")]
	public static void BuildAndroid()
	{
		string sdk = Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
		EditorPrefs.SetString("AndroidSdkRoot", sdk);
		string error = BuildPipeline.BuildPlayer(scenes, "build/UnityRemoteNG-Android.apk", BuildTarget.Android, BuildOptions.None).ToString();

		if (error != null && error.Length > 0) {
			throw new Exception("Build failed: " + error);
		}
	}
}
