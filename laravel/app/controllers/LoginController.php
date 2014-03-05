<?php

class LoginController extends Controller
{
    function login()
    {
        $openid = new LightOpenID('http://renx.vicious-reality.com/login');
        
        if(!$openid->mode) 
        {
            $openid->identity = 'http://steamcommunity.com/openid/?l=english';
            return Redirect::to($openid->authUrl());
        }
        else
        {
            if($openid->validate()) 
            {
                $split = explode('/', $openid->identity);
                
                $steamid = $split[5];
                
                $p = Player::where('steamid', '=', $steamid)->take(1)->get()->first();
                
                if(!$p)
                    return Redirect::to('/')->withErrors(array('You need to play on our server at least once first!'));
                
                Auth::login($p);
                
                return Redirect::to('/')->with('success', 'You have logged in!');
            }
        }
    }
    
    function logout()
    {
        if(Auth::check())
            Auth::logout();
            
        return Redirect::to('/');
    }
}
