<?php

/*
|--------------------------------------------------------------------------
| Application Routes
|--------------------------------------------------------------------------
|
| Here is where you can register all of the routes for an application.
| It is a breeze. Simply tell Lumen the URIs it should respond to
| and give it the Closure to call when that URI is requested.
|
*/
$router->post('authenticate', 'UserController@authenticate');

$router->group(['prefix' => 'api','middleware' => 'auth'], function () use ($router) {
    //barangay routes
    $router->get('barangay', 'BarangayController@get');

    //user routes
    $router->get('user', 'UserController@get');
});