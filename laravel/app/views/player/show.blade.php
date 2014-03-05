@extends('layout')

@section('content')
    <div class="row">
        <div class="col-lg-8">
            <div class="panel panel-default">
                <div class="panel-heading" id="online_status">Profile - {{ $p->online ? '<span style="color: #19FF19;">Online</span>' : '<span style="color: #D12B2B;">Offline</span>' }}</div>
                <table class="table table-bordered table-striped">
                    <tbody>
                        <tr>
                            <td rowspan="5">
                                <img src="{{ $p->hasPhoto() ? $p->getPhoto() : '/photo.jpg'}}" alt="Alternate Text" class="img-responsive" style="margin: auto;"/>
                            </td>
                        </tr>
                        <tr>
                            <th>Name</th>
                            <td><a href="{{ route('player', $p->steamid) }}">{{ $p->name }}</a> (<a href="http://steamcommunity.com/profiles/{{ $p->steamid }}" target="_blank">Steam Profile</a>)</td>
                        </tr>
                        <tr>
                            <th>Level</th>
                            <td id="plevel">{{ $p->plevel }}</td>
                        </tr>
                        <tr>
                            <th>Current Experience</th>
                            <td id="experience">{{ $p->experience }}</td>
                        </tr>
                        <tr>
                            <th>Required Experience</th>
                            <td id="req_experience">{{ $p->CalculateRequiredExperience() }}</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="progress progress-striped active">
                                    <div class="progress-bar progress-bar-info" role="progressbar" id="experience_bar" aria-valuenow="{{ $p->CalculateRequiredExperiencePercent() }}" aria-valuemin="0" aria-valuemax="100" style="width: {{ $p->CalculateRequiredExperiencePercent() }}%">
                                        <span class="sr-only">{{ $p->CalculateRequiredExperiencePercent() }}% Complete</span>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel panel-default">
                <div class="panel-heading">General stats</div>
                <table class="table table-bordered table-striped">
                    <tbody>
                        <tr>
                            <th>Kills</th>
                            <td id="kills">{{ number_format($p->kills) }}</td>
                        </tr>
                        <tr>
                            <th>Deaths</th>
                            <td id="deaths">{{ number_format($p->deaths) }}</td>
                        </tr>
                        <tr>
                            <th>Headshots</th>
                            <td id="headshots">{{ number_format($p->headshots) }} ({{ number_format($p->kills > 0 ? ($p->headshots / $p->kills) * 100 : $p->headshots, 2) }}% of kills)</td>
                        </tr>
                        <tr>
                            <th>Longest Killstreak</th>
                            <td id="killstreak">{{ $p->killstreak }}</td>
                        </tr>
                        <tr>
                            <th>KDR</th>
                            <td>{{ number_format($p->deaths > 0 ? $p->kills / $p->deaths : $p->kills, 2) }}</td>
                        </tr>
                        <tr>
                            <th>Buildings Ruined</th>
                            <td id="buildings">{{ number_format($p->buildings) }}</td>
                        </tr>
                        <tr>
                            <th>Vehicles Wrecked</th>
                            <td id="destroyed">{{ number_format($p->destroyed) }}</td>
                        </tr>
                        <tr>
                            <th>Longest Deathstreak</th>
                            <td id="deathstreak">{{ $p->deathstreak }}</td>
                        </tr>
                        <tr>
                            <th>Roadkill</th>
                            <td id="roadkill">{{ number_format($p->runover) }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
@stop

@section('scripts')
    <script type="text/javascript" src="/js/player.js"></script>
@stop