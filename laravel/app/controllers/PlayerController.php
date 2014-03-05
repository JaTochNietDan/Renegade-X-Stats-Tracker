<?php

class PlayerController extends Controller
{
    function show($id)
    {
        $p = Player::where('steamid', '=', $id)->take(1)->get()->first();
        
        if(!$p)
            return Redirect::to(route('home'))->withErrors(array('Player with that ID not found!'));
        
        if(Request::ajax())
            return Response::json($p);
        
        return View::make('player.show', array('p' => $p));
    }
    
    function search()
    {
        $players = Player::where('name', 'LIKE', '%'.Input::get('name').'%')->take(20)->get();
        
        return View::make('player.search', array('players' => $players));
    }
    
    function edit($id)
    {
        if(!Auth::check() || $id != Auth::getUser()->steamid)
            return Redirect::to('/')->withErrors(array('You can\'t edit this account!'));
        
        return View::make('player.edit');
    }
    
    function save($id)
    {
        if(!Auth::check() || $id != Auth::getUser()->steamid)
            return Redirect::to('/')->withErrors(array('You can\'t edit this account!'));
        
        $rules = array(
            'email' => 'email'
        );
        
        Auth::getUser()->update(Input::all());
        
        return Redirect::to(route('editprofile', $id))->with('success', 'Saved profile settings!');
    }
    
    function upload($id)
    {
        if(!Auth::check() || $id != Auth::getUser()->steamid)
            return Redirect::to('/')->withErrors(array('You can\'t edit this account!'));
        
        $rules = array(
            'file' => 'required|size:1024'
        );
        
        if(!Input::hasFile('photo'))
            return Redirect::to(route('editprofile', $id))->withErrors(array('You did not select a file!'));
        
        $file = Input::file('photo');
        
        if(strtolower($file->getClientOriginalExtension()) != 'png')
            return Redirect::to(route('editprofile', $id))->withErrors(array('You can only upload PNGs!'));
        
        smart_resize_image($file->getRealPath(), 120, 120, false, public_path('uploads/').$id.'.png', true, false, 100);
        
        return Redirect::to(route('editprofile', $id))->with('success', 'Uploaded photo!');
    }
}
