import React from 'react';

export const getStatusDisplay = (statusId: number) => {
  switch (statusId) {
    case 1:
      return <span className="badge bg-warning">Pending</span>;
    case 2:
      return <span className="badge bg-danger">Declined</span>;
    case 3:
      return <span className="badge bg-success">Started</span>;
    case 4:
      return <span className="badge bg-danger">Aborted</span>;
    case 5:
      return <span className="badge bg-secondary">Archived</span>;
    default:
      return <span className="badge bg-secondary">Unknown</span>;
  }
};