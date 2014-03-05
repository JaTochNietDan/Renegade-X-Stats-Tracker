<?php

Route::get('/', array('uses' => 'MainController@index', 'as' => 'home'));

Route::get('/player/{id}', array('uses' => 'PlayerController@show', 'as' => 'player'));

Route::get('/search', array('uses' => 'PlayerController@search', 'as' => 'search'));
Route::get('/losers', array('uses' => 'MainController@losers', 'as' => 'losers'));

Route::get('/login', array('uses' => 'LoginController@login', 'as' => 'login'));
Route::get('/logout', array('uses' => 'LoginController@logout', 'as' => 'logout'));

Route::get('/player/{id}/edit', array('uses' => 'PlayerController@edit', 'as' => 'editprofile'));
Route::post('/player/{id}/edit/upload', array('before' => 'csrf', 'uses' => 'PlayerController@upload', 'as' => 'uploadphoto'));
Route::post('/player/{id}/edit', array('before' => 'csrf', 'uses' => 'PlayerController@save', 'as' => 'saveprofile'));