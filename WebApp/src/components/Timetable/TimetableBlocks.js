import React from 'react';
import PropTypes from 'prop-types';
import _ from 'lodash';
import lcm from 'mlcm';
import TimetableBlockContainer from '../../containers/TimetableBlockContainer/TimetableBlockContainer';

export function groupByTimeBlocks(timetable) {
  if (!_.isArray(timetable) || _.isEmpty(timetable)) {
    return [];
  }
  const ordered = _.orderBy(timetable, [
    'day',
    'startBlock',
    'endBlock',
    'courseCode'
  ]);
  const groupsByDay = _.groupBy(ordered, 'day');
  const groupsByDayAndStartBlock = _.map(groupsByDay, g =>
    _.groupBy(g, 'startBlock')
  );
  const groupsByStartBlock = _.flatten(
    _.map(groupsByDayAndStartBlock, _.values)
  );
  return groupsByStartBlock;
}

const TimetableBlocks = props => {
  const groupedBlocks = groupByTimeBlocks(props.items);
  if (_.isEmpty(groupedBlocks)) {
    return null;
  }
  const rowHeight = lcm(_.map(groupedBlocks, _.size));
  const blocks = [];
  let lastDay = 0;
  let lastEndBlock = 0;
  let marginTop = 0;
  _.each(groupedBlocks, group => {
    const groupSize = _.size(group);
    const blockHeight = rowHeight / groupSize;
    _.each(group, (block, i) => {
      const startLine = (block.day - 1) * rowHeight + i * blockHeight + 1;
      const classes = [];
      if (lastDay != block.day) {
        lastDay = block.day;
        lastEndBlock = 0;
        marginTop = 0;
      } else if (
        block.startBlock < lastEndBlock &&
        lastEndBlock < block.endBlock
      ) {
        marginTop += 5;
      } else {
        marginTop = 0;
      }
      lastEndBlock = block.endBlock;
      if (groupSize > 2) {
        classes.push('extrasmall');
      } else if (groupSize > 1) {
        classes.push('small');
      }
      blocks.push(
        <TimetableBlockContainer
          key={`${block.courseCode}-${block.day}x${block.startBlock}`}
          {...block}
          cssClasses={classes}
          style={{
            gridColumn: `${block.startBlock} / ${block.endBlock}`,
            gridRow: `${startLine} / ${startLine + blockHeight}`,
            marginTop: `${marginTop}%`
          }}
        />
      );
    });
  });
  const style = {
    gridTemplateColumns: `repeat(${props.columns}, 1fr)`,
    gridTemplateRows: `repeat(${props.rows * rowHeight}, 1fr)`
  };
  return (
    <div className="blocks" style={style}>
      {blocks}
    </div>
  );
};

TimetableBlocks.propTypes = {
  columns: PropTypes.number.isRequired,
  rows: PropTypes.number.isRequired,
  items: PropTypes.arrayOf(PropTypes.shape({}))
};

TimetableBlocks.defaultProps = {
  items: []
};

export default TimetableBlocks;
