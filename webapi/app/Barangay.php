<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Barangay extends Model
{
    protected $table = 'barangay';    
    protected $hidden = ['created_at', 'updated_at'];    
}