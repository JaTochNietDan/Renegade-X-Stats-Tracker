@extends('layout')

@section('content')
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">Search Results ({{ count($players) }})</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Kills</th>
                            <th>Deaths</th>
                            <th>Headshots</th>
                        </tr>
                    </thead>
                    <tbody>
                    @if(count($players) == 0)
                        <tr>
                            <td colspan="4">Nobody found with that information!</td>
                        </tr>
                    @endif
                    @foreach($players as $p)
                        <tr>
                            <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                            <td>{{ $p->kills }}</td>
                            <td>{{ $p->deaths }}</td>
                            <td>{{ $p->headshots }}</td>
                        </tr>
                    @endforeach
                    </tbody>
                </table>
            </div>
        </div>
    </div>
@stop