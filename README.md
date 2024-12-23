# Project Introduction
    
The Event Management System is a platform designed to simplify the creation, management, and attendance of events. It provides tools for organizers to host events, manage tickets, and organize sessions efficiently while ensuring scalability and high performance using advanced architectural principles.

# Requirements

## Functional Requirements

- Given a long URL, create a short URL
- Given a short URL, redirect to a long URL

## Non-Functional Requirements

- Very low latency
- Very high availability

## Not Covered

- Updating of URLs
- Deleting of URLs

# Modeling Database

create keyspace url_shortener with replication = {'class': 'SimpleStrategy', 'replication_factor': 1};

create table urls
(
    id         UUID primary key,
    long_url   TEXT,
    short_url  TEXT,
    created_at TIMESTAMP
);

