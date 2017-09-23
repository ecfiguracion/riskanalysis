<?php

namespace App\Http\Controllers;

use App\Barangay;

class BarangayController extends Controller
{
    public function get()
    {
        return response()->json(Barangay::all());       
    }
}
