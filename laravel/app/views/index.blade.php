@extends('layout')

@section('content')
    <div class="row">
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Biggest Skull Stompers</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Skulls Decimated</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::orderBy('headshots', 'DESC')->take(10)->get() as $p)
                            <tr>
                                <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                <td>{{ number_format($p->headshots) }}</td>
                            </tr>
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Serial Killers</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Killstreak</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::orderBy('killstreak', 'DESC')->take(10)->get() as $p)
                            <tr>
                                <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                <td>{{ $p->killstreak }}</td>
                            </tr>
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Biggest Showoffs</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>KDR</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::getKDR() as $p)
                            @if($p->kdr)
                                <tr>
                                    <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                    <td>{{ number_format($p->kdr, 2) }}</td>
                                </tr>
                            @endif
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Biggest Killers</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Enemies Eradicated</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::orderBy('kills', 'DESC')->take(10)->get() as $p)
                            <tr>
                                <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                <td>{{ number_format($p->kills) }}</td>
                            </tr>
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Best Vehicle Crushers</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Vehicles Obliterated</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::orderBy('destroyed', 'DESC')->take(10)->get() as $p)
                            <tr>
                                <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                <td>{{ number_format($p->destroyed) }}</td>
                            </tr>
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">Most Destructive</div>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Buildings Leveled</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach(Player::orderBy('buildings', 'DESC')->take(10)->get() as $p)
                            <tr>
                                <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a></td>
                                <td>{{ $p->buildings }}</td>
                            </tr>
                        @endforeach
                    </tbody>
                </table>
            </div>
        </div>
    </div>
@stop