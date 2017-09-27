﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginHttpRequest : MonoBehaviour {

	private LoginController loginController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Registe (string account, string password, LoginController controller){
		loginController = controller;
		StartCoroutine (HttpRegiste(account, password));
	}

	public void Login(string account, string password, LoginController controller){
		loginController = controller;
		StartCoroutine (HttpLogin(account, password));
	}
		
	IEnumerator HttpRegiste(string account, string password){

		Dictionary<string, string> pars = new Dictionary<string, string>();
		pars.Add ("name", account);
		pars.Add ("password", password);

		UnityWebRequest request = UnityWebRequest.Post ("http://127.0.0.1:8080/api/user/register", pars);
		yield return request.Send ();

		if (request.isNetworkError) {
			Debug.Log (request.error);
		} else {
			if (request.responseCode == 200) {
				string text = request.downloadHandler.text;
				Debug.Log ("response " + text);

				loginController.RegisteSuccess ();
			} else {
				Debug.Log ("responseCode is not OK");
			}
		}
	}

	IEnumerator HttpLogin(string account, string password){

		Dictionary<string, string> pars = new Dictionary<string, string>();
		pars.Add ("name", account);
		pars.Add ("password", password);

		UnityWebRequest request = UnityWebRequest.Post ("http://127.0.0.1:8080/api/user/login", pars);
		yield return request.Send ();

		if (request.isNetworkError) {
			Debug.Log (request.error);
		} else {
			if (request.responseCode == 200) {
				string text = request.downloadHandler.text;
				Debug.Log ("response " + text);

				ResponseModel<UserModel> responseModel = JsonUtility.FromJson<ResponseModel<UserModel>> (text);
				if(responseModel.code == 200){


					string token = ((UserModel)responseModel.data).authAccessToken;
					PlayerPrefs.SetString (PrefDefine.PP_USER_SESSION_TOKEN, token);

					loginController.LoginSuccess ();
				}
			} else {
				Debug.Log ("responseCode is not OK");
			}
		}
	}
}
