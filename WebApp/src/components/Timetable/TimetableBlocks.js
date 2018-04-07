import React from 'react';
import PropTypes from 'prop-types';
import { each, flatten, groupBy, map, orderBy, size, values } from 'lodash';
import lcm from 'mlcm';
import TimetableBlock from './TimetableBlock';

const TimetableBlocks = (props) => {
  const dayGroups = groupBy(orderBy(props.items, ['day', 'startBlock', 'endBlock', 'name']), 'day');
  const dayStartBlockGroups = map(dayGroups, group => groupBy(group, 'startBlock'));
  const clusters = flatten(map(dayStartBlockGroups, values));
  const rowHeight = lcm(map(clusters, size));
  const blocks = [];
  each(clusters, (cluster) => {
    const clusterSize = size(cluster)
    const blockHeight = rowHeight / clusterSize;
    each(cluster, (block, i) => {
      const startLine = ((block.day - 1) * rowHeight) + (i * blockHeight) + 1;
      const classes = [block.type];
      if (clusterSize > 3) {
        classes.push('small');
      } else if (clusterSize > 2) {
        classes.push('medium');
      }
      blocks.push(
        <TimetableBlock
          key={`${block.name}-${block.day}x${block.startBlock}`}
          name={block.name}
          room={block.room}
          teacher={block.tearcher}
          cssClasses={classes}
          style={{
            gridColumn: `${block.startBlock} / ${block.endBlock}`,
            gridRow: `${startLine} / ${startLine + blockHeight}`,
          }}
        />
      );
    });
  });
  const style = {
    gridTemplateColumns: `repeat(${props.columns}, 1fr)`,
    gridTemplateRows: `repeat(${props.rows * rowHeight}, 1fr)`,
  };
  return (
    <div className="blocks" style={style}>{blocks}</div>
  );
};

TimetableBlocks.propTypes = {
  columns: PropTypes.number.isRequired,
  rows: PropTypes.number.isRequired,
  items: PropTypes.arrayOf(PropTypes.shape({})),
};

TimetableBlocks.defaultProps = {
  items: [],
};

export default TimetableBlocks;