<?php

namespace App\Http\Controllers;

use App\User;

class UserController extends Controller
{
    public function get()
    {
        return response()->json(User::all());       
    }
}
