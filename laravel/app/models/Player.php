<?php

use Illuminate\Auth\UserInterface;

class Player extends Eloquent implements UserInterface
{
    protected $table = 'players';
    protected $fillable = array('email');
    public $timestamps = false;
    
    protected $hidden = array('email', 'ip');
    protected $appends = array('exp_percent', 'req_experience');
    
    public static function getKDR($total = 10)
    {
        return Player::select(DB::raw('*, kills / deaths AS kdr'))
                ->orderBy('kdr', 'desc')
                ->take($total)
                ->get();
    }
    
    public function CalculateRequiredExperience()
    {
        $basexp = 1000;

        return $basexp + ($this->plevel * 320);
    }
    
    public function CalculateRequiredExperiencePercent()
    {
        $required = $this->CalculateRequiredExperience();
        
        return ($this->experience / $required) * 100;
    }
    
    public function getExpPercentAttribute()
    {
        return $this->CalculateRequiredExperiencePercent();    
    }
    
    public function getReqExperienceAttribute()
    {
        return $this->CalculateRequiredExperience();    
    }
    
    public function getAuthIdentifier()
	{
		return $this->getKey();
	}

	public function getAuthPassword()
	{
		return $this->password;
	}
    
    public function hasPhoto()
    {
        return File::exists(public_path('uploads/'.$this->steamid.'.png'));
    }
    
    public function getPhoto()
    {
        return '/uploads/'.$this->steamid.'.png';
    }
}
