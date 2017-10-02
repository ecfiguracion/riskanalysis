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
    $router->get('barangay/get[/{id}]', 'BarangayController@get');
    $router->post('barangay/save', 'BarangayController@save');
    $router->post('barangay/delete', 'BarangayController@delete');

    //user routes
    $router->get('user', 'UserController@get');
});

$router->get('foo', function () {
    return 'hello world';
});