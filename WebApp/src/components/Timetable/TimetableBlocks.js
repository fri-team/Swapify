import React from 'react';
import PropTypes from 'prop-types';
import {
  each, flatten, groupBy, isArray, isEmpty,
  map, orderBy, size, values,
} from 'lodash';
import lcm from 'mlcm';
import TimetableBlockContainer from '../../containers/TimetableBlockContainer/TimetableBlockContainer';

export function groupByTimeBlocks(timetable) {
  if (!isArray(timetable) || isEmpty(timetable)) {
    return [];
  }
  const ordered = orderBy(timetable, ['day', 'startBlock', 'endBlock', 'courseShortcut']);
  const groupsByDay = groupBy(ordered, 'day');
  const groupsByDayAndStartBlock = map(groupsByDay, g => groupBy(g, 'startBlock'));
  const groupsByStartBlock = flatten(map(groupsByDayAndStartBlock, values));
  return groupsByStartBlock;
}

const TimetableBlocks = (props) => {
  const groupedBlocks = groupByTimeBlocks(props.items);
  if (isEmpty(groupedBlocks)) {
    return null;
  }
  const rowHeight = lcm(map(groupedBlocks, size));
  const blocks = [];
  let lastDay = 0;
  let lastEndBlock = 0;
  let marginTop = 0;
  each(groupedBlocks, (group) => {
    const groupSize = size(group);
    const blockHeight = rowHeight / groupSize;
    each(group, (block, i) => {
      const startLine = ((block.day - 1) * rowHeight) + (i * blockHeight) + 1;
      const classes = [];
      if (lastDay != block.day) {
        lastDay = block.day;
        lastEndBlock = 0;
        marginTop = 0;
      } else if (block.startBlock < lastEndBlock && lastEndBlock < block.endBlock) {
        marginTop += 5;
      } else {
        marginTop = 0;
      }
      lastEndBlock = block.endBlock;
      if (groupSize > 3) {
        classes.push('small');
      } else if (groupSize > 2) {
        classes.push('medium');
      }
      blocks.push(
        <TimetableBlockContainer
          key={`${block.courseShortcut}-${block.day}x${block.startBlock}`}
          {...block}
          cssClasses={classes}
          style={{
            gridColumn: `${block.startBlock} / ${block.endBlock}`,
            gridRow: `${startLine} / ${startLine + blockHeight}`,
            marginTop: `${marginTop}%`,
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
