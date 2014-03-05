@extends('layout')

@section('content')
<div class="row">
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Most Deaths</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Owned Count</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::orderBy('deaths', 'DESC')->take(10)->get() as $p)
                            <tr>
                                <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                <td>{{ $p->deaths }}</td>
                            </tr>
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Longest Deathstreak</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Deathstreak</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::orderBy('deathstreak', 'DESC')->take(10)->get() as $p)
                            <tr>
                                <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                <td>{{ $p->deathstreak }}</td>
                            </tr>
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Most Roadkilled</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Roadkill Count</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::orderBy('runover', 'DESC')->take(10)->get() as $p)
                            <tr>
                                <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                <td>{{ $p->runover }}</td>
                            </tr>
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
    </div>   
@stop