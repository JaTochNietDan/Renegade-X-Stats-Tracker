<?php

class MainController extends Controller
{
	function index()
	{
		return View::make('index');
	}
	
	function losers()
	{
		return View::make('losers');
	}
}