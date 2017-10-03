<?php

namespace App\Http\Controllers;

use App\Barangay;
use Illuminate\Http\Request;

class BarangayController extends Controller
{
    public function get($id)
    {
        return Barangay::findOrFail($id);       
    }

    public function search(Request $request)
    {
        $filter = $request->Text;
        if ($filter)
            return Barangay::where('name','like','%' . $filter . '%')->paginate(5);
        else
            return Barangay::paginate(5);
    }    

    public function save(Request $request) {

        //validate first
        $this->validate($request, [
            'name' => 'required',
            'longitude' => 'numeric',
            'latitude' => 'numeric'
        ]);
            
        //if id = 0 means new record
        //if id > 0 means will do update
        $id = $request->id;
        if ($id > 0) {
            $barangay = Barangay::find($request->id);    
        } else {
            $barangay = new Barangay;
        }

        //assign all passed values to object
        $data = $request->except('api_token');
        foreach($data as $field => $value) {            
            $barangay[$field] = $value;
        }

        //finally we save
        $barangay->save();

        return 'record save successfully';
    } 

    public function delete(Request $request) {

        //assign all passed values to object
        $data = $request->except('api_token');        
        $ids = [];

        foreach($data as $field => $value) {            
            $ids[] = $value;
        }
                
        Barangay::destroy($ids);

        return 'record successfully remove';
    }     
}
