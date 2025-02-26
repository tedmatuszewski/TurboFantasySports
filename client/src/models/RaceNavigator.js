import races from '../data/races.json';

function getRaceByKey(key) {
    return races.find(race => race.key === key);
}

function isNextUpcomingRace(race) {
    const now = new Date();
    const raceDate = new Date(race.date);

    if (raceDate <= now) return false;

    const upcomingRace = races.filter(r => new Date(r.date) > now)[0];
    
    return race.key === upcomingRace.key;
}

function getNextUpcomingRaceIndex() {
    for(let i = 0; i<races.length; i++) {
        if (isNextUpcomingRace(races[i])) {
            return (i+1);
        }
    }
}

function getNextUpcomingRace() {
    for(let i = 0; i<races.length; i++) {
        if (isNextUpcomingRace(races[i])) {
            return races[i];
        }
    }
}

function getPreviousRace() {
    const now = new Date();
    let previousRace = null;

    for(let i = 0; i < races.length; i++) {
        const raceDate = new Date(races[i].date);

        if (raceDate < now) {
            previousRace = races[i];
        } else {
            break; // Since races are sorted by date, we can stop once we find a race in the future
        }
    }

    return previousRace;
}

export { getNextUpcomingRace, getRaceByKey, getNextUpcomingRaceIndex,isNextUpcomingRace,getPreviousRace };