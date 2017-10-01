<?php

namespace App\Http\Controllers;

use App\User;
use Illuminate\Http\Request;

class UserController extends Controller
{
    public function get()
    {
        return User::all();       
    }

    public function authenticate(Request $request) {
        $this->validate($request, [
            'email' => 'required|email',
            'password' => 'required'
        ]);
            
        $user = User::where([
            'email' => $request->email,
            'password' => $request->password
        ])->first();
        
        if ($user)
            return $user->api_token;
        else
            return null;
    }
}
