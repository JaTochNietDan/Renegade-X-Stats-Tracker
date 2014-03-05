@extends('layout')

@section('content')
    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-default">
                <div class="panel-heading">Profile Photo</div>
                <table class="table table-bordered table-striped">
                    <tbody>
                        <tr>
                            <td rowspan="4">
                                <img src="{{ Auth::getUser()->hasPhoto() ? Auth::getUser()->getPhoto() : '/photo.jpg'}}" alt="Alternate Text" class="img-responsive" />
                            </td>
                        </tr>
                        <tr>
                            <td>Upload your photo (Must be png, max 1 MB)</td>
                        </tr>
                        {{ Form::open(array('route' => array('uploadphoto', Auth::getUser()->steamid), 'files' => true)) }}
                        <tr>
                            <td>
                                <input type="file" name="photo" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="submit" class="btn btn-success" value="Upload" />
                            </td>
                        </tr>
                        {{ Form::close() }}
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="panel panel-default">
                <div class="panel-heading">Profile</div>
                <div class="panel-body">
                    {{ Form::open(array('route' => array('saveprofile', Auth::getUser()->steamid), 'class' => 'form-horizontal')) }}
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Email</label>
                            <div class="col-sm-10">
                                {{ Form::email('email', Auth::getUser()->email, array('class' => 'form-control')) }}
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                                <button type="submit" class="btn btn-default">Save</button>
                            </div>
                        </div>
                    {{ Form::close() }}
                </div>
            </div>
        </div>
    </div>
@stop